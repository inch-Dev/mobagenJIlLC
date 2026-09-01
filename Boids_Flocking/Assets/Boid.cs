using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour
{

    Vector2 velocity;

    BoidSettings boidSettings;
    LineRenderer lineRenderer;
    CircleCollider2D circleCollider;

    List<Boid> neighbors = new List<Boid>();

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        boidSettings = BoidSettings.instance;
        lineRenderer = GetComponent<LineRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        RandomizeMovement();
    }

    // Update is called once per frame
    void Update()
    {
        DrawDebug();
        NeighborhoodRadius();
        Move();
    }

    void Move()
    {
        transform.position += new Vector3(velocity.x * boidSettings.movementSpeed, velocity.y * boidSettings.movementSpeed, 0f);

        Vector3 steerValue = Vector3.zero;

        if (boidSettings.alignmentRule)
            steerValue += Alignment();
        if (boidSettings.cohesionRule)
            steerValue += Cohesion();
        if (boidSettings.separationRule)
            steerValue += Separation();

        transform.position += steerValue;

        //Add steervalue to transform.position
    }

    void RandomizeMovement()
    {
        velocity.x = Random.Range(-1f, 1f);
        velocity.y = Random.Range(-1f, 1f);
    }

    void NeighborhoodRadius()
    {
        circleCollider.radius = boidSettings.neighborhoodRadius;
    }

    Vector3 Alignment()
    {
        Vector3 steer = Vector3.zero;
        Vector3 neighborDirection = Vector3.zero;


        //Average direction vector for neighborhood
        //Add fricition

        return steer;
    }

    Vector3 Cohesion()
    {
        Vector3 steer = Vector3.zero;

        Vector3 neighborCenter = Vector3.zero;

        foreach(Boid boid in neighbors)
        {
            neighborCenter += boid.transform.position;
        }

        neighborCenter /= neighbors.Count;

        Debug.Log($"NeighborCenter:{neighborCenter}");

        steer = transform.position - neighborCenter;

        return steer;
    }

    Vector3 Separation()
    {
        Vector3 steer = Vector2.zero;

        foreach(Boid boid in neighbors)
        {
            Vector3 distance = (transform.position - boid.transform.position);
            distance = distance.normalized * (boidSettings.separationWeight / distance.magnitude);

            steer += distance;
        }

        if(neighbors.Count > 0)
            steer = steer * (1 / neighbors.Count);

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

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.GetComponent<Boid>())
            neighbors.Add(collision.GetComponent<Boid>());
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.GetComponent<Boid>())
            neighbors.Remove(collision.GetComponent<Boid>());
	}

}
