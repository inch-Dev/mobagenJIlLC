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
        Vector3 randomPos = new Vector3(Random.Range(-15f, 15f), Random.Range(-8f, 8f), 0f);
        GameObject newBoid = GameObject.Instantiate(boidPF, randomPos, Quaternion.identity);
        boids.Add(newBoid.GetComponent<Boid>());
    }
}
