using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Catcher : Agent, IGameStatable
{
    [HideInInspector] public static Catcher instance;
    public void HandleGameState()
    {
        switch (GameManager.instance.GetGameState())
        {
            case GameState.GAME_START:
                BootStrap();
                break;
            case GameState.CATCHER:
                if (isAgent)
                {
                    Debug.Log("Catcher search");
					Search();
                }
                break;
        }
    }

    void Start()
    {
        if(instance == null)
            instance = this;
    }
	public override void BootStrap()
	{
		for (int i = 0; i < World.instance.GetXSize(); i++)
		{
			for (int j = 0; j < World.instance.GetYSize(); j++)
			{
				if (i == 0 || j == 0 || i == World.instance.GetXSize() - 1 || j == World.instance.GetYSize() - 1)
					_goals.Add(World.instance.GetPointAt(new Vector2(i, j)));
			}
		}

		_start = World.instance.GetPointAt(new Vector2((int)(World.instance.GetXSize() / 2), (int)(World.instance.GetYSize() / 2)));
		_frontier.Add(_start);
		_cameFromMap.Add(_start, _start);
		_costSoFar.Add(_start, 0);
	}

	public override void Search()
	{
		if (wasSearched)
		{
			_start = Cat.instance.GetPosition();
			//For goals find goal that is closest to the cat
			Debug.Log($"Setting start to {_start}");

			_frontier.Clear();
			_frontierHead = null;
			_cameFromMap.Clear();
			_costSoFar.Clear();
			_path.Clear();


			_frontier.Add(_start);
			_cameFromMap.Add(_start, _start);
			_costSoFar.Add(_start, 0);

		}
		while (_frontier.Count > 0)
		{
			Step();
		}

		if (_frontier.Count <= 0)
		{
			World.instance.ResetPoints();
			//GameManager.instance.SetGameState(GameState.CAT_WIN);
		}

	}
	public override void Behavior()
	{
		if (_path.Count <= 0)
		{
            Debug.Log("Catcher game over");
			GameManager.instance.SetGameState(GameState.CAT_WIN);
			base.Behavior();
			return;
		}
		_position = _path[_path.Count- 1];
        _position.SetPointType(PointType.WALL);

		base.Behavior();
        GameManager.instance.SetGameState(GameState.CAT);
	}
}
