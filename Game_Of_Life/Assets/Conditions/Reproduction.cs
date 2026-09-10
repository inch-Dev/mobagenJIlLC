using JetBrains.Annotations;
using UnityEngine;

public class Reproduction : Condition
{
	public override bool Test(Agent agent)
	{
		World world = agent.GetWorld();
		Vector2Int agentPos = world.GetPositionOfAgent(agent);
		//Debug.Log($"Agent position:{agentPos}");
		//Wrap around



		//3 Nearby Neighbors
		int neighborCount = 0;
		bool nearbyNeighbor = false;




		return false; return false; 
	}
}
