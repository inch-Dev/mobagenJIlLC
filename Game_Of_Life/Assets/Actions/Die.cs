using UnityEngine;

public class Die : Action
{
	public override void Execute(Agent agent)
	{
		agent.SetRenderer(false);

		Debug.Log($"Agent at {World.instance.GetPositionOfAgent(agent)} died");
	}
}
