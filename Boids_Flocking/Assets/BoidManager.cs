using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
public class BoidManager : MonoBehaviour
{
    [SerializeField] BoidSettings boidSettings;
    [SerializeField] GameObject boidPF;
    List<Boid> boids = new List<Boid>();

    // Update is called once per frame
    void Update()
    {
        if (boids.Count < boidSettings.numBoids)
            SpawnBoids();
        if (boids.Count > boidSettings.numBoids)
            RemoveBoids();

            Debug.Log($"Boids:{boids.Count}, Num Boids:{boidSettings.numBoids}");
    }

    void SpawnBoids()
    {    
        for(int i = 0; boids.Count < boidSettings.numBoids; i++)
        {
            SpawnBoid();
        }
    }

    void RemoveBoids()
    {
        for(int i = 0; boids.Count > boidSettings.numBoids; i++)
        {
            foreach(Boid boid in boids)
            {
                boid.RemoveNeighbor(boids[i]);
                
            }
            boids.RemoveAt(i);
            Destroy(boids[i].gameObject);
            
        }
        Debug.Log("Remove");
    }

    void SpawnBoid()
    {
        Vector3 randomPos = new Vector3(Random.Range(boidSettings.horizontalMargins.x, boidSettings.horizontalMargins.y), Random.Range(boidSettings.verticalMargins.x, boidSettings.verticalMargins.y), 0f);
        GameObject newBoid = GameObject.Instantiate(boidPF, randomPos, Quaternion.identity);
        newBoid.GetComponentInChildren<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 0f, 1f, 1f,1f, 1f, 1f);
        boids.Add(newBoid.GetComponent<Boid>());
    }

    public void RandomizeBoids()
    {
        foreach(Boid boid in boids)
        {
            boid.RandomizeMovement();
            boid.RandomizePosition();
        }
    }
}
