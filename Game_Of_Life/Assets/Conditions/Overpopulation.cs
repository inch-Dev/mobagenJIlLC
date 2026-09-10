using UnityEngine;

public class Overpopulation : Condition
{
	public override bool Test(Agent agent) 
	{
		World world = agent.GetWorld();
		Vector2Int agentPos = world.GetPositionOfAgent(agent);
		//Wrap around



		//Four or more neighbors
		int neighborCount = 0;




		return false;
	}
}
