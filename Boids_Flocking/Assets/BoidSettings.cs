using UnityEngine;

public class BoidSettings : MonoBehaviour
{
    [HideInInspector] public static BoidSettings instance;
    public int numBoids;
    public float neighborhoodRadius;

    public bool showAcceleration;
    public bool showRadius;
    public bool showRules;

    public bool separationRule;
    public float separationWeight;
    public float targetSeparation;

    public bool cohesionRule;
    public float cohesionWeight;

    public bool alignmentRule;
    public float alignmentWeight;

    public float movementSpeed = 1f;
    public float maxAcceleration = 1f;


	private void Start()
	{
		if(instance == null)
            instance = this;
	}
}
