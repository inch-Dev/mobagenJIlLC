using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class BoidSettings : MonoBehaviour
{
    [HideInInspector] public static BoidSettings instance;
    public int numBoids = 125;

    public void SetNumBoids(Slider newNum){ numBoids = (int)newNum.value; }
    public float neighborhoodRadius = 2;
    public void SetNeighborhoodRadius(Slider newRadius){  neighborhoodRadius = newRadius.value; }

    [Header("Debug Drawing")]

    public bool showAcceleration = false;

    public void ShowAcceleration(Toggle show){ showAcceleration = show.isOn; }

    public bool showRadius = false;

    public void ShowRadius(Toggle show){ showRadius = show.isOn; }
    public bool showRules = false;
    public void ShowRules(Toggle show){ showRules = show.isOn; }

    [Header("Separation")]
    public bool separationRule = false;
    public void SetSeparationRule(Toggle rule){ separationRule = rule; }
    public float separationWeight = 25f;
    public void SetSeparationWeight(Slider weight){ separationWeight = weight.value; }
    public float targetSeparation = 0.5f;
    public void SetTargetSeparation(Slider target){ targetSeparation = target.value; }

    [Header("Cohesion")]
    public bool cohesionRule = false;
    public void SetCohesionRule(Toggle rule){ cohesionRule = rule.isOn; }
    public float cohesionWeight = 20f;
    public void SetCohesionWeight(Slider weight) { cohesionWeight = weight.value; }

    [Header("Alignment")]
    public bool alignmentRule = false;

    public void SetAlignmentRule(Toggle rule){ alignmentRule = rule.isOn; }
    public float alignmentWeight;
    public void SetAlignmentWeight(Slider weight) { alignmentWeight = weight.value; }

    [Header("Movement Values")]
    public float movementSpeed = 1f;

    public void SetMoveSpeed(Slider speed){  movementSpeed = speed.value; }
    public float maxAcceleration = 1f;

    public void SetMaxAccel(Slider accel) { maxAcceleration = accel.value; }


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
