using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class World : MonoBehaviour
{
    [HideInInspector] public static World instance;
    [SerializeField] Vector2Int gridSize;
    [SerializeField] float offset;
    [SerializeField] GameObject agentPF;

    Agent[,] m_agents;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;
		m_agents = new Agent[gridSize.x, gridSize.y];
		GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateGrid()
    {
        for(int i = 0; i < gridSize.x; i++)
        {
            for(int j = 0; j < gridSize.y; j++)
            {
                SpawnCellAgent(new Vector2Int(i, j));
            }
        }
    }

    void SpawnCellAgent(Vector2Int index)
    {
        Vector2 newPosition = new Vector2(transform.position.x + index.x * offset, transform.position.y + index.y * offset);
        GameObject newAgent = GameObject.Instantiate(agentPF, newPosition, Quaternion.identity);
        newAgent.transform.parent = transform;
        newAgent.GetComponent<Agent>().SetWorld(this);

		m_agents[index.x, index.y] = newAgent.GetComponent<Agent>();
    }

}
