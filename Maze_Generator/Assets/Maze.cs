using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class Maze : MonoBehaviour, IStateable
{
    public void HandleState(State state)
    {
        switch (state)
        {
            case State.PAUSED:
                canStep = false;
                break;
            case State.PLAY:
                canStep = true;
                break;
        }
    }

    int[] randomNumbers = { 72, 99, 56, 34, 43, 62, 31, 4, 70, 22, 6, 65, 96, 71, 29, 9, 98, 41, 90, 7, 30, 3, 97, 49, 63, 88, 47, 82, 91, 54, 74, 2, 86, 14, 58, 35, 89, 11, 10, 60, 28, 21, 52, 50, 55, 69, 76, 94, 23, 66, 15, 57, 44, 18, 67, 5, 24, 33, 77, 53, 51, 59, 20, 42, 80, 61, 1, 0, 38, 64, 45, 92, 46, 79, 93, 95, 37, 40, 83, 13, 12, 78, 75, 73, 84, 81, 8, 32, 27, 19, 87, 85, 16, 25, 17, 68, 26, 39, 48, 36 };
    int randomIndex = 0;
    int GetRandomNumber()
    {
        if (randomIndex >= randomNumbers.Length)
            randomIndex = 0;

        int randomNumber = randomNumbers[randomIndex];
        randomIndex++;

        return randomNumber;
        
    }
    
    [SerializeField] int sideSize;
    [SerializeField] float offset;
    [SerializeField] GameObject vertexPF;
    List<Vertex> mazeVertices = new List<Vertex>();
   Vertex GetVertexAt(Vector2Int index)
    {
        foreach(Vertex vertex in mazeVertices)
        {
            if (vertex.GetIndex() == index)
                return vertex;
        }
        return null;
    }
    List<Vertex> searchVertices = new List<Vertex>();
    Vertex searchHead;

    [SerializeField] float stepTimeSeconds;
    float stepTimeElapsed;

    bool canStep;

    List<Vertex> GetNeighbors(Vertex vertex)
    {
        List<Vertex> neighbors = new List<Vertex>();

        //Search in directions
        List<Vector2Int> directions = new List<Vector2Int>();
        directions.Add(Vector2Int.up);
        directions.Add(Vector2Int.right);
        directions.Add(Vector2Int.down);
        directions.Add(Vector2Int.left);
        
        //Search in clockwise order
        for(int i = 0; i < directions.Count; i++)
        {
            if (GetVertexAt(vertex.GetIndex() + directions[i]))
                neighbors.Add(GetVertexAt(vertex.GetIndex() + directions[i]));  
        }

        return neighbors;
    }


    private void Start()
    {
        GenerateMaze();
        searchHead = GetVertexAt(new Vector2Int(0, 0));
    }

    private void Update()
    {
        if (canStep)
        {
            stepTimeElapsed += Time.deltaTime;
            if (stepTimeElapsed > stepTimeSeconds)
            {
                stepTimeElapsed = 0;
                Step();
            }
        }
    }



    void GenerateMaze()
    {
        for(int i = 0; i < sideSize; i++)
        {
            for(int j = 0; j < sideSize; j++)
            {
                GenerateVertex(new Vector2(i * offset, j * offset));
            }
        }

        
    }

    void GenerateVertex(Vector2 position)
    {
        GameObject newVertexObject = GameObject.Instantiate(vertexPF, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        newVertexObject.transform.parent = this.transform;

        Vertex newVertex = newVertexObject.GetComponent<Vertex>();
        newVertex.SetIndex(new Vector2Int((int)(position.x / offset), (int)(position.y / offset)));
        newVertex.RandomizeWalls();
    }

    public void Step()
    {
        Debug.Log("Stepping");
        //Get Stack -> Update Colors
        foreach(Vertex vertex in searchVertices)
        {
            vertex.GetSpriteRenderer().color = Color.green;
        }

        searchHead.GetSpriteRenderer().color = Color.blue;

        //Move through stack

        //Get neighbors
        List<Vertex> neighbors = GetNeighbors(searchHead);

        //Random select search
        if(neighbors.Count > 1)
        {
            int randomIndex = GetRandomNumber();
            int choosenNeighborIndex = randomIndex % neighbors.Count;
            neighbors[choosenNeighborIndex].Search();
            neighbors[choosenNeighborIndex].GetSpriteRenderer().color = Color.blue;
        }

        else if(neighbors.Count > 0) 
        {
            neighbors[0].Search();
            neighbors[0].GetSpriteRenderer().color = Color.blue;
        }

        else
        {
            //Turn around
            //Remove search head from stack -> update color
            //Update search head to second to last in stack
        }

    }
}
