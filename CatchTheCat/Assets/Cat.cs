using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cat : Agent
{
	public void HandleGameState()
	{
		switch (GameManager.instance.GetGameState())
		{
			case GameState.GAME_START:
				break;
			case GameState.CAT:
				Search();
				break;
			case GameState.GAME_OVER:
				isActive = false;
				break;
		}
	}
	//Multiple goals of outside cells

	//Look for closest goal and move towards

	//Create path and move one step towards

	// Start is called before the first frame update

	public override void BootStrap()
	{
		//Goals are edge pieces

		for(int i = 0; i < World.instance.GetXSize(); i++)
		{
			for(int j = 0; j < World.instance.GetYSize(); j++)
			{
				if (i == World.instance.GetXSize() - 1 || j == World.instance.GetYSize() - 1)
					_goals.Add(World.instance.GetPointAt(new Vector2(i, j)));
			}
		}


		_start = World.instance.GetPointAt(new Vector2( (int)(World.instance.GetXSize() / 2),(int)(World.instance.GetYSize() / 2)));
		_frontier.Add(_start);
		_cameFromMap.Add(_start,_start);
		_costSoFar.Add(_start, _start.Priority);
	}

	public override void CreatePath()
	{
		base.CreatePath();
	}

	public override void Search()
	{
		if (_frontier.Count <= 0)
			return;


		if (_frontierHead != null && CheckForGoal(_frontierHead))
		{
			_goal = _frontierHead;
			Debug.Log($"Found goal at {_frontierHead.Coordinates}");
			_frontier.Clear();
			CreatePath();


			return;
		}

		//Debug.Log("Searching...");

		foreach (var point in _frontier)
		{
			point.Frontiered();
		}


		_frontierHead = GetLowestPriority(_frontier);
		_frontierHead.FrontierHead();



		_frontier.Remove(_frontierHead);
		{
			_frontierHead.Reached();
		}



		List<Point> neighbors = GetNeighbors(_frontierHead);
		foreach (var neighbor in neighbors)
		{
			if (neighbor.Type == PointType.WALL)
			{
				//Debug.Log("Found wall");
				continue;
			}
			float newCost = _costSoFar[_frontierHead] + World.instance.GetCostOf(_frontierHead, neighbor);
			neighbor.Neighbored();
			//Debug.Log($"Found neighbor of {_frontierHead}:{neighbor}");
			//New Cost

			if (!_costSoFar.ContainsKey(neighbor) || newCost < _costSoFar[neighbor])
			{
				_costSoFar[neighbor] = newCost;
				neighbor.Priority = newCost + World.instance.GetCostOf(GetClosestGoal(neighbor), neighbor); //Need heuristic
				_frontier.Add(neighbor);
				_cameFromMap[neighbor] = _frontierHead;
			}
			else
			{
				neighbor.Frontiered();
			}

		}
	}
}
