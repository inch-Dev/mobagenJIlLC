using UnityEngine;

public class BoidSettings : MonoBehaviour
{
    [HideInInspector] public static BoidSettings instance;
    int numBoids;
    public float neighborhoodRadius;

    public bool showAcceleration;
    public bool showRadius;
    public bool showRules;

    public bool seperationRule;
    public float seperationWeight;
    public float targetSeparation;

    public bool cohesionRule;
    public float cohesionWeight;

    public bool alignmentRule;
    public float alignmentWeight;

	private void Start()
	{
		if(instance == null)
            instance = this;
	}
}
