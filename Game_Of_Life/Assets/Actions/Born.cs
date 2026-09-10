using UnityEngine;

public class Born : Action
{
	public override void Execute(Agent agent)
	{
		agent.GetDeadRenderer().enabled = false;
		agent.GetAliveRenderer().enabled = true;
	}
}
