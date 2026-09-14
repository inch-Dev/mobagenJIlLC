using UnityEngine;
using System.Collections.Generic;

public class Maze : MonoBehaviour
{

    [SerializeField] int sideSize;
    [SerializeField] float offset;
    [SerializeField] GameObject vertexPF;
    List<Vertex> mazeVertices = new List<Vertex>();
    

    void GenerateMaze()
    {
        for(int i = 0; i < sideSize; i++)
        {
            for(int j = 0; j < sideSize; j++)
            {
                GenerateCell(new Vector2(i * offset, j * offset));
            }
        }
    }

    void GenerateCell(Vector2 position)
    {
        GameObject newVertexObject = GameObject.Instantiate(vertexPF, new Vector3(position.x, position.y, 0f), Quaternion.identity);
        newVertexObject.transform.parent = this.transform;

        Vertex newVertex = newVertexObject.GetComponent<Vertex>();

        newVertex.RandomizeState();
    }

    public void Step()
    {

    }
}
