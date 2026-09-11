using UnityEngine;

public class Born : Action
{
	public override void Execute(Agent agent)
	{
		agent.SetRenderer(true);

		Debug.Log($"Agent at {World.instance.GetPositionOfAgent(agent)} was born");
	}
}
