using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
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
        newBoid.GetComponentInChildren<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 0f, 1f, 1f,1f, 1f, 1f);
        boids.Add(newBoid.GetComponent<Boid>());
    }
}
