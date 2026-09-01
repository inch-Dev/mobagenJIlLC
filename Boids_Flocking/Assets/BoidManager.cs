using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class BoidManager : MonoBehaviour
{
    [SerializeField] BoidSettings boidSettings;
    [SerializeField] GameObject boidPF;
    List<Boid> boids = new List<Boid>();

    // Update is called once per frame
    void Update()
    {
        if(boidSettings != null && boids.Count < boidSettings.numBoids)
            SpawnBoids();

        Debug.Log($"Boid settings:{boidSettings}");
    }

    void SpawnBoids()
    {    
        for(int i = boids.Count - 1; i < boidSettings.numBoids; i++)
        {
            SpawnBoid();
        }
    }

    void SpawnBoid()
    {
        GameObject newBoid = GameObject.Instantiate(boidPF);
        boids.Add(newBoid.GetComponent<Boid>());
    }
}
