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
    [SerializeField] Toggle cohesion;
    [SerializeField] Toggle alignment;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
