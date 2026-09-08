using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
public class Agent : MonoBehaviour, IStateable
{

	public void HandleState(GameState state)
	{

	}
	
	[SerializeField] SpriteRenderer m_aliveRenderer;
	[SerializeField] SpriteRenderer m_deadRenderer;
	StateMachine m_stateMachine;
	World m_World;
	public void SetWorld(World world){ m_World = world; }
	public World GetWorld(){ return m_World; }

	private void Start()
	{
		m_stateMachine = GetComponent<StateMachine>();
	}
}
