using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Boid : MonoBehaviour
{
    Vector3 velocity;
    Vector3 acceleration;
    public Vector3 GetVelocity(){ return velocity; }

    BoidSettings boidSettings;
    LineRenderer lineRenderer;
    CircleCollider2D circleCollider;

    List<Boid> neighbors = new List<Boid>();
    List<GameObject> obstacles = new List<GameObject>();

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        boidSettings = BoidSettings.instance;
        lineRenderer = GetComponent<LineRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        RandomizeMovement();
    }

	private void FixedUpdate()
	{
		DrawDebug();
		DetectNeighbors();
		MarginClip();
		Move();
	}

	void DetectNeighbors()
    {
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(transform.position, boidSettings.neighborhoodRadius);

        obstacles.Clear();
        neighbors.Clear();

        foreach(Collider2D neighborCollider in neighborColliders)
        {
            if (neighborCollider.TryGetComponent(out Boid boid) && neighborCollider != circleCollider)
                neighbors.Add(boid);
            else if (boidSettings.collisonAvoidance &&  neighborCollider != GetComponent<CircleCollider2D>())
                obstacles.Add(neighborCollider.gameObject);
            //Debug.Log(neighbors.Count);
        }

        Debug.Log($"Obstacles:{obstacles.Count}");
    }

    void MarginClip()
    {
        if(transform.position.x < boidSettings.horizontalMargins.x)
        {
            transform.position = new Vector3(boidSettings.horizontalMargins.y, transform.position.y, 0);
        }

        if(transform.position.x > boidSettings.horizontalMargins.y)
        {
			transform.position = new Vector3(boidSettings.horizontalMargins.x, transform.position.y, 0);
		}

		if (transform.position.y < boidSettings.verticalMargins.x)
        {
			transform.position = new Vector3(transform.position.x, boidSettings.verticalMargins.y, 0);
		}

		if (transform.position.y > boidSettings.verticalMargins.y)
        {
			transform.position = new Vector3(transform.position.x, boidSettings.verticalMargins.x, 0);
		}
	}

    Vector3 EdgeAvoidance()
    {
        if(transform.position.magnitude > boidSettings.maxConstraintRadius - boidSettings.neighborhoodRadius)
        {
            float distance = boidSettings.maxConstraintRadius - transform.position.magnitude;

            float strength = 1f / (distance * distance);

            Vector3 avoidDirection = -transform.position.normalized;

            return avoidDirection * strength * boidSettings.avoidanceWeight;
        }

        return Vector3.zero;
    }

    void WrapMovement()
    {
        float newX = transform.position.x;
        float newY = transform.position.y;


        Vector3 screenPos = Camera.main.WorldToViewportPoint(GetComponentInChildren<SpriteRenderer>().bounds.center);
        Vector3 screenExtents = Camera.main.WorldToViewportPoint(GetComponentInChildren<SpriteRenderer>().bounds.extents);

        if(screenPos.x > 1f + screenExtents.x)
        {
            newX = -screenExtents.x;
            newY = screenPos.y;
        }

        if(screenPos.x < 0f - screenExtents.x)
        {
            newX= 1f + screenExtents.x;
            newY = screenPos.y;
        }

        if(screenPos.y > 1f + screenExtents.y)
        {
            newY = -screenExtents.y;
            newX = screenPos.x;
        }

        if(screenPos.y < 0f - screenExtents.y)
        {
            newY = 1f + screenExtents.y;
            newX = screenPos.x;
        }

		transform.position = Camera.main.ViewportToWorldPoint(new Vector3(newX, newY, 0));
	}

	void Move()
    {
        if (boidSettings.alignmentRule)
            acceleration += Alignment();
        if (boidSettings.cohesionRule)
            acceleration += Cohesion();
        if (boidSettings.separationRule)
            acceleration += Separation();
        //acceleration += EdgeAvoidance();

        Debug.Log($"Alignment:{Alignment()}, Cohesion:{Cohesion()}, Separation:{Separation()}");

        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, boidSettings.maxAcceleration);

        transform.position += velocity * Time.fixedDeltaTime;

        acceleration = Vector3.zero;

        //Look In Direction
        if (velocity != Vector3.zero)
        {
            //Debug.Log($"Velocity:{velocity}");
            transform.up = velocity.normalized;
        }
    }

    void RandomizeMovement()
    {
        velocity = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized * boidSettings.movementSpeed;
    }

    Vector3 Alignment()
    {
        Vector3 steer = Vector3.zero;

        if (neighbors.Count == 0)
            return steer;

        foreach(Boid neighbor in neighbors)
        {
            steer += neighbor.GetVelocity();
        }

        if (neighbors.Count > 0)
        {
            steer /= neighbors.Count;
        }

        steer = (steer - GetVelocity()) * boidSettings.alignmentWeight;
        return steer;
    }

    Vector3 Cohesion()
    {
        Vector3 steer = Vector3.zero;

        if (neighbors.Count == 0)
            return steer;

		// Average out all positions
		foreach (Boid boid in neighbors)
        {
            steer += boid.transform.position;
        }

        if (neighbors.Count > 0)
            steer *= (1f / neighbors.Count);


        steer = boidSettings.cohesionWeight * (steer - transform.position);
		//Debug.Log($"Cohesion Force:{steer}");

		return steer;
    }

    Vector3 Separation()
    {
        Vector3 steer = Vector3.zero;

		foreach (Boid boid in neighbors)
		{
			Vector3 offset = (transform.position - boid.transform.position);

            float distance = offset.magnitude;

            if(distance > 0f && distance < boidSettings.targetSeparation)
                steer += offset.normalized / distance;
		}

		if (neighbors.Count > 0)
			steer = steer * (1f / neighbors.Count);

		steer *= boidSettings.separationWeight;

		return steer;
    }


    void DrawDebug()
    {
		if (boidSettings.showAcceleration)
		{

		}

		if (boidSettings.showRadius)
		{
            DrawRadius();
		}

		if (boidSettings.showRules)
		{

		}
	}

    void DrawAcceleration() { }

    void DrawRadius()
    {
        float lineWidth = .1f;
        int segments = 360;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = segments + 1;

        int pointCount = segments + 1;
        Vector3[] points = new Vector3[pointCount];

        for(int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * boidSettings.neighborhoodRadius, Mathf.Cos(rad) * boidSettings.neighborhoodRadius, 0f);
        }

        lineRenderer.SetPositions(points);


    }

    void DrawRules() { }

	//private void OnTriggerEnter2D(Collider2D collision)
	//{
 //       if (collision.GetComponent<Boid>())
 //           neighbors.Add(collision.GetComponent<Boid>());
	//}

	//private void OnTriggerExit2D(Collider2D collision)
	//{
 //       if (collision.GetComponent<Boid>())
 //           neighbors.Remove(collision.GetComponent<Boid>());
	//}

}
