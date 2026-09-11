using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
public class Agent : MonoBehaviour, IStateable
{

	public void HandleState(GameState state)
	{
		
	}

	SpriteRenderer m_spriteRenderer;
	public SpriteRenderer GetRenderer(){ return m_spriteRenderer; }
	public void SetRenderer(bool isAlive)
	{
		if (isAlive)
			m_spriteRenderer.color = Color.yellow;
		else
			m_spriteRenderer.color = Color.black;
	}
	StateMachine m_stateMachine;
	public StateMachine GetStateMachine(){ return  m_stateMachine; }
	World m_World;
	public void SetWorld(World world){ m_World = world; }
	public World GetWorld(){ return m_World; }

	private void Awake()
	{
		m_stateMachine = GetComponent<StateMachine>();
		m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
	}
}
