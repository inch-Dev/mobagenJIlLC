using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BoidUI : MonoBehaviour
{
    public void SetNumBoids(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.numBoids.ToString("F2"); }
    public void SetNeighborhoodRadius(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.neighborhoodRadius.ToString("F2"); }
    public void SetMoveSpeed(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.movementSpeed.ToString("F2"); }
    public void SetMaxAccel(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.maxAcceleration.ToString("F2"); }
    public void SetSeparationWeight(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.separationWeight.ToString("F2"); }
    public void SetSeparationDistance(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.targetSeparation.ToString("F2"); }
    public void SetCohesionWeight(TextMeshProUGUI tf){  tf.text = BoidSettings.instance.cohesionWeight.ToString("F2"); }
    public void SetAlignmentWeight(TextMeshProUGUI tf){ tf.text = BoidSettings.instance.alignmentWeight.ToString("F2"); }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
