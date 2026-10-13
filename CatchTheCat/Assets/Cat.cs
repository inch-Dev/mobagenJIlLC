using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Cat : Agent, IGameStatable
{
	[HideInInspector] public static Cat instance;
	[SerializeField] GameObject _catVisual;
	public void HandleGameState()
	{
		switch (GameManager.instance.GetGameState())
		{
			case GameState.GAME_START:
				break;
			case GameState.CAT:
				if (isAgent)
					Search();
				break;
		}
	}

	private void Start()
	{
		if (instance == null)
			instance = this;
	}

	public override void BootStrap()
	{
		for(int i = 0; i < World.instance.GetXSize(); i++)
		{
			for(int j = 0; j < World.instance.GetYSize(); j++)
			{
				if (j == 0 || i == 0 || i == World.instance.GetXSize() - 1 || j == World.instance.GetYSize() - 1)
					_goals.Add(World.instance.GetPointAt(new Vector2(i, j)));
			}
		}

		_start = World.instance.GetPointAt(new Vector2( (int)(World.instance.GetXSize() / 2),(int)(World.instance.GetYSize() / 2)));
		_frontier.Add(_start);
		_cameFromMap.Add(_start,_start);
		_costSoFar.Add(_start, 0);
	}
	public override void Behavior()
	{
		//Set cat position to next step in path

		if (_path.Count <= 0)
		{
			Debug.Log("Cat game over");
			GameManager.instance.SetGameState(GameState.CATCHER_WIN);
			base.Behavior();
			return;
		}
		_position = _path[0];
		_catVisual.transform.position = _position.gameObject.transform.position;
		Debug.Log($"Moving to {_position}");

		base.Behavior();

		GameManager.instance.SetGameState(GameState.CATCHER);
	}
}
