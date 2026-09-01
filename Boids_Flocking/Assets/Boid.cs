using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Boid : MonoBehaviour
{

    Vector2 velocity;
    public Vector2 GetVelocity(){ return velocity; }

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
        DetectNeighbors();
        Move();
    }

    void DetectNeighbors()
    {
        Collider2D[] neighborColliders = Physics2D.OverlapCircleAll(transform.position, boidSettings.neighborhoodRadius);

        neighbors.Clear();

        foreach(Collider2D neighborCollider in neighborColliders)
        {
            if (neighborCollider.TryGetComponent(out Boid boid) && neighborCollider != circleCollider)
                neighbors.Add(boid);
            Debug.Log(neighbors.Count);
        }
    }

	Vector3 steerValue;
	void Move()
    {
        //transform.position += new Vector3(velocity.x * boidSettings.movementSpeed, velocity.y * boidSettings.movementSpeed, 0f) * Time.deltaTime;


        if (boidSettings.alignmentRule)
            steerValue += Alignment();
        if (boidSettings.cohesionRule)
            steerValue += Cohesion();
        if (boidSettings.separationRule)
            steerValue += Separation();

        transform.position += steerValue * Time.deltaTime;

        //Add steervalue to transform.position
    }

    void RandomizeMovement()
    {
		steerValue.x = Random.Range(-1f, 1f) * boidSettings.movementSpeed;
		steerValue.y = Random.Range(-1f, 1f) * boidSettings.movementSpeed;
    }



    Vector3 Alignment()
    {
        Vector3 steer = Vector3.zero;

        if (neighbors.Count == 0)
            return steer;

        foreach(Boid neighbor in neighbors)
        {
            steer += new Vector3(neighbor.GetVelocity().x, neighbor.GetVelocity().y, 0f);
        }

        if(neighbors.Count > 0)
            steer *= (1f / neighbors.Count);

        steer = (steer - new Vector3(velocity.x, velocity.y, 0f)) * boidSettings.alignmentWeight;

        Debug.Log($"Alignment Force:{steer}");
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
            steer *= (1f / neighbors.Count);


        steer = boidSettings.cohesionWeight * steer;
		Debug.Log($"Cohesion Force:{steer}");

		return steer;
    }

    Vector3 Separation()
    {
        Vector3 steer = Vector2.zero;

        foreach(Boid boid in neighbors)
        {
            Vector3 distance = (transform.position - boid.transform.position);
            distance = distance.normalized * (boidSettings.targetSeparation / distance.magnitude);

            steer += distance;
        }

        if(neighbors.Count > 0)
            steer = steer * (1 / neighbors.Count);

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
