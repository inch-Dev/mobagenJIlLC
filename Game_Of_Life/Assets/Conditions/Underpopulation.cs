using UnityEngine;

public class Underpopulation : Condition
{
	public override bool Test(Agent agent)
	{
        World world = agent.GetWorld();
        Vector2Int agentPos = world.GetPositionOfAgent(agent);
        //Wrap around



        //No nearby neighbors
        bool nearbyNeighbor = false;

        
        
        
        return false;
	}
}
