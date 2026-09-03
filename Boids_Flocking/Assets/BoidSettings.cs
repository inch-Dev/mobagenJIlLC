using Unity.VisualScripting;
using UnityEngine;

public class BoidSettings : MonoBehaviour
{
    [HideInInspector] public static BoidSettings instance;
    public int numBoids = 125;

    public void SetNumBoids(int newNum){ numBoids = newNum; }
    public float neighborhoodRadius = 2;
    public void SetNeighborhoodRadius(float newRadius){  neighborhoodRadius = newRadius; }

    [Header("Debug Drawing")]

    public bool showAcceleration = false;

    public bool showRadius = false;
    public bool showRules = false;

    [Header("Separation")]
    public bool separationRule = false;
    public float separationWeight = 25f;
    public float targetSeparation = 0.5f;

    [Header("Cohesion")]
    public bool cohesionRule = false;
    public float cohesionWeight = 20f;

    [Header("Alignment")]
    public bool alignmentRule = false;
    public float alignmentWeight;

    [Header("Movement Values")]
    public float movementSpeed = 1f;
    public float maxAcceleration = 1f;


    [Header("Position Clamping")]

    public Vector2 horizontalMargins = new Vector2(-20f,20f);

    public Vector2 verticalMargins = new Vector2(-8f, 8f);


	private void Start()
	{
		if(instance == null)
            instance = this;
	}

    public void  ResetBoidDefaultvalues()
    {

    }
}
