using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Boid : MonoBehaviour
{

    [SerializeField] LineRenderer radiusRenderer;
    [SerializeField] LineRenderer accelRenderer;
    [SerializeField] LineRenderer separationRenderer;
    [SerializeField] LineRenderer cohesionRenderer;
    [SerializeField] LineRenderer alignmentRenderer;
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

    Vector3 GetAcceleration()
    {
        Vector3 acceleration = Vector3.zero;
		if (boidSettings.alignmentRule)
			acceleration += Alignment() * boidSettings.alignmentWeight;
		if (boidSettings.cohesionRule)
			acceleration += Cohesion() * boidSettings.cohesionWeight;
		if (boidSettings.separationRule)
			acceleration += Separation() * boidSettings.separationWeight;

        return acceleration;
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

        velocity += acceleration * Time.fixedDeltaTime * boidSettings.movementSpeed;
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
        DrawAcceleration(boidSettings.showAcceleration);

        DrawRadius(boidSettings.showRadius);

		DrawRules(boidSettings.showRules);
	}

    void DrawAcceleration(bool showAccel)
    {
        if(!showAccel)
        {
            accelRenderer.positionCount = 0;
            return;
        }

		float lineWidth = .1f;
		accelRenderer.useWorldSpace = false;
		accelRenderer.startWidth = lineWidth;
	    accelRenderer.endWidth = lineWidth;

		int segments = 2;
		accelRenderer.positionCount = segments;
		Vector3[] points = new Vector3[segments];


        points[0] = Vector3.zero;
		points[1] = GetAcceleration().normalized;

		accelRenderer.SetPositions(points);

	}

    void DrawRadius(bool showRadius)
    {
        if(!showRadius)
        {
            radiusRenderer.positionCount = 0;
            return;
        }
        //Toggle off lineRenderer if no debug
        float lineWidth = .1f;
        int segments = 360;
        radiusRenderer.useWorldSpace = false;
        radiusRenderer.startWidth = lineWidth;
        radiusRenderer.endWidth = lineWidth;
        radiusRenderer.positionCount = segments + 1;

        int pointCount = segments + 1;
        Vector3[] points = new Vector3[pointCount];

        for(int i = 0; i < pointCount; i++)
        {
            float rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * boidSettings.neighborhoodRadius, Mathf.Cos(rad) * boidSettings.neighborhoodRadius, 0f);
        }

        radiusRenderer.SetPositions(points);


    }

    void DrawRules(bool showRules)
    {
        if(!showRules)
        {
            separationRenderer.positionCount = 0;
            cohesionRenderer.positionCount = 0;
            alignmentRenderer.positionCount = 0;
            return;
        }

		float lineWidth = .1f;

		if (boidSettings.separationRule)
        {
            separationRenderer.useWorldSpace = false;
            separationRenderer.startWidth = lineWidth;
            separationRenderer.endWidth = lineWidth;

            int segments = 2;
            separationRenderer.positionCount = segments;
            Vector3[] points = new Vector3[segments];


			points[0] = Vector3.zero;
            points[1] = Separation().normalized;

            separationRenderer.SetPositions(points);
        }

        else
        {
            separationRenderer.positionCount = 0;
        }


        if(boidSettings.cohesionRule)
        {
			cohesionRenderer.useWorldSpace = false;
			cohesionRenderer.startWidth = lineWidth;
			cohesionRenderer.endWidth = lineWidth;

			int segments = 2;
			cohesionRenderer.positionCount = segments;
			Vector3[] points = new Vector3[segments];


			points[0] = Vector3.zero;
			points[1] = Cohesion().normalized;

			cohesionRenderer.SetPositions(points);
		}

        else
        {
            cohesionRenderer.positionCount = 0;
        }

        if(boidSettings.alignmentRule)
        {
			alignmentRenderer.useWorldSpace = false;
			alignmentRenderer.startWidth = lineWidth;
			alignmentRenderer.endWidth = lineWidth;

            int segments = 2;
			alignmentRenderer.positionCount = segments;
			Vector3[] points = new Vector3[segments];


            points[0] = Vector3.zero;
			points[1] = Alignment().normalized;

			alignmentRenderer.SetPositions(points);
		}

        else
        {
            alignmentRenderer.positionCount = 0;
        }
    }

}
