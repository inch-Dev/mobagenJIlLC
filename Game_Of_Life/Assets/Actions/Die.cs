using UnityEngine;

public class Die : Action
{
	public override void Execute(Agent agent)
	{
		agent.GetAliveRenderer().enabled = false;
		agent.GetDeadRenderer().enabled = true;
	}
}
