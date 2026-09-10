using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Reproduction : Condition
{
	public override bool Test(Agent agent)
	{
		Vector2Int agentPos = World.instance.GetPositionOfAgent(agent);

		//Four or more neighbors
		int aliveNeighborCount = 0;
		List<Vector2Int> directionChecks = new List<Vector2Int>();
		directionChecks.Add(Vector2Int.up);
		directionChecks.Add(Vector2Int.down);
		directionChecks.Add(Vector2Int.left);
		directionChecks.Add(Vector2Int.right);

		Vector2Int upLeft = new Vector2Int(-1, 1);
		directionChecks.Add(upLeft);
		Vector2Int upRight = new Vector2Int(1, 1);
		directionChecks.Add(upRight);
		Vector2Int downLeft = new Vector2Int(-1, -1);
		directionChecks.Add(downLeft);
		Vector2Int downRight = new Vector2Int(1, -1);
		directionChecks.Add(downRight);

		foreach (Vector2Int directionCheck in directionChecks)
		{
			if (World.instance.GetAgentAt(agentPos + directionCheck).GetStateMachine().GetCurrentState() is Alive)
			{
				aliveNeighborCount++;
			}
		}

		if (aliveNeighborCount == 3)
			return true;


		return false;
	}
}
