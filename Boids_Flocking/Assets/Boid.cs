using UnityEngine;

public class Boid : MonoBehaviour
{

    Vector2 velocity;

    BoidSettings boidSettings;
    LineRenderer lineRenderer;
    CircleCollider2D circleCollider;

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
        Move();
    }

    void Move()
    {
        transform.position += new Vector3(velocity.x * boidSettings.movementSpeed, velocity.y * boidSettings.movementSpeed, 0f);

        Vector2 steerValue = Vector2.zero;

        if (boidSettings.alignmentRule)
            steerValue += Alignment() * boidSettings.alignmentWeight;
        if (boidSettings.cohesionRule)
            steerValue += Cohesion() * boidSettings.cohesionWeight;
        if (boidSettings.separationRule)
            steerValue += Separation() * boidSettings.separationWeight;

        //Add steervalue to transform.position
    }

    void RandomizeMovement()
    {
        velocity.x = Random.Range(0f, 1f);
        velocity.y = Random.Range(0f, 1f);
    }

    void NeighborhoodRadius()
    {
        circleCollider.radius = boidSettings.neighborhoodRadius;
    }

    Vector2 Alignment()
    {
        Vector2 steer = Vector2.zero;

        return steer;
    }

    Vector2 Cohesion()
    {
        Vector2 steer = Vector2.zero;

        return steer;
    }

    Vector2 Separation()
    {
        Vector2 steer = Vector2.zero;

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
        Debug.Log("Detected");
	}

}
