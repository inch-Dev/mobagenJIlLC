using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cat : Agent
{
	//Multiple goals of outside cells

	//Look for closest goal and move towards

	//Create path and move one step towards

	// Start is called before the first frame update

	public override void BootStrap()
	{
		base.BootStrap();

		//middle of world

		_start = World.instance.GetPointAt(new Vector2( (int)(World.instance.GetXSize() / 2),(int)(World.instance.GetYSize() / 2)));
		_frontier.Add(_start);
		_cameFromMap.Add(_start,_start);
		_costSoFar.Add(_start, _start.Priority);
	}

	public override void CreatePath()
	{
		base.CreatePath();
	}

	public override void Step()
	{
		base.Step();
	}
}
