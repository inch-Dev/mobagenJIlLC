using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
public class World : MonoBehaviour, IStateable
{

    public void HandleState(GameState state)
    {
        switch (state)
        {
            case GameState.CREATE:
                isRunning = false;
                break;
            case GameState.PLAY:
                isRunning = true;
                break;
        }

    }
    [HideInInspector] public static World instance;
    [SerializeField] Vector2Int gridSize;
    [SerializeField] float offset;
    bool isRunning = false;
    [SerializeField] float stepIntervalSeconds;
    float stepIntervalTimeElapsed = 0f;
    [SerializeField] GameObject agentPF;

    Agent[,] m_agents;
    public Agent GetAgentAt(Vector2Int position){ return m_agents[position.x, position.y]; }
    public Vector2Int GetPositionOfAgent(Agent agent)
    {
        foreach(Agent worldAgent in m_agents)
        {
            if(worldAgent == agent)
            {
                return new Vector2Int((int)(agent.transform.position.x / offset), (int)(agent.transform.position.y / offset));
            }
        }
        return Vector2Int.zero;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance == null)
            instance = this;
		m_agents = new Agent[gridSize.x, gridSize.y];
		GenerateGrid();
    }

	private void FixedUpdate()
	{
		if(isRunning)
        {
            stepIntervalTimeElapsed += Time.deltaTime;
            if(stepIntervalTimeElapsed >= stepIntervalSeconds)
            {
                Step();
            }
        }
	}

	void Step()
    {
        foreach( Agent agent in m_agents)
        {
            Debug.Log(GetPositionOfAgent(agent));
            agent.GetStateMachine().UpdateState(agent);
        }
    }

    void GenerateGrid()
    {
        for(int i = 0; i < gridSize.x; i++)
        {
            for(int j = 0; j < gridSize.y; j++)
            {
                SpawnCellAgent(new Vector2Int(i, j));
                //Debug.Log($"Spawning at :{new Vector2Int(i, j)}");
            }
        }
    }

    void SpawnCellAgent(Vector2Int index)
    {
        Vector2 newPosition = new Vector2(transform.position.x + index.x * offset, transform.position.y + index.y * offset);
        GameObject newAgent = GameObject.Instantiate(agentPF, newPosition, Quaternion.identity);
        newAgent.transform.parent = transform;
        newAgent.GetComponent<Agent>().SetWorld(this);
        newAgent.GetComponent<Agent>().GetStateMachine().SetCurrentState(newAgent.GetComponent<Dead>());

        newAgent.name = "Agent" + index.ToString();

		m_agents[index.x, index.y] = newAgent.GetComponent<Agent>();
    }

}
