using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BoidUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] Slider numBoids;
    [SerializeField] Slider neighborhoodRadius;
    [Header("Display")]
    [SerializeField] Toggle showAcceleration;
    [SerializeField] Toggle showRadius;
    [SerializeField] Toggle showRules;

    [Header("Rules")]
    [SerializeField] Toggle separation;
    [SerializeField] Slider separationWeight;
    [SerializeField] Slider separationDistance;
    [SerializeField] Toggle cohesion;
    [SerializeField] Slider cohesionWeight;
    [SerializeField] Toggle alignment;
    [SerializeField] Slider alignmentWeight;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
