using UnityEngine;

public class Underpopulation : Condition
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override bool Test(Agent agent)
	{
        World world = agent.GetWorld();

        
        return false;
	}
}
