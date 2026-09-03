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

	void Move()
    {
        if (boidSettings.alignmentRule)
            acceleration += Alignment() * boidSettings.alignmentWeight;
        if (boidSettings.cohesionRule)
            acceleration += Cohesion() * boidSettings.cohesionWeight;
        if (boidSettings.separationRule)
            acceleration += Separation() * boidSettings.separationWeight;
        //acceleration += EdgeAvoidance();

        Debug.Log($"Alignment:{Alignment()}, Cohesion:{Cohesion()}, Separation:{Separation()}");

        velocity += acceleration * Time.fixedDeltaTime;
        velocity = Vector3.ClampMagnitude(velocity, boidSettings.maxAcceleration);

        transform.position += velocity * Time.fixedDeltaTime;

        Debug.Log($"Velocity:{velocity},Position:{transform.position}");

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
            steer += boid.transform.position - transform.position;
        }

        if (neighbors.Count > 0)
            steer /= neighbors.Count;


        steer = steer.normalized;

		return steer;
    }

    Vector3 Separation()
    {
        Vector3 steer = Vector3.zero;

		foreach (Boid boid in neighbors)
		{

            Vector3 direction = transform.position - boid.transform.position;
            float magnitude = direction.magnitude;
            Vector3 distance = direction / magnitude;

            if(Vector3.Distance(transform.position, boid.transform.position) < boidSettings.targetSeparation && Vector3.Distance(transform.position, boid.transform.position) > 0f)
            {
                steer += distance / magnitude;
            }
		}

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
        //Toggle off lineRenderer if no debug
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

}
