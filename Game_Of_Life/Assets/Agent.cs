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
	
	[SerializeField] SpriteRenderer m_aliveRenderer;
	public SpriteRenderer GetAliveRenderer(){ return m_aliveRenderer; }
	public void SetAliveRenderer(bool isOn){ m_aliveRenderer.enabled = isOn; }
	[SerializeField] SpriteRenderer m_deadRenderer;
	public SpriteRenderer GetDeadRenderer(){  return m_deadRenderer; }
	public void SetDeadRenderer(bool isOn){  m_deadRenderer.enabled = isOn; }
	StateMachine m_stateMachine;
	public StateMachine GetStateMachine(){ return  m_stateMachine; }
	World m_World;
	public void SetWorld(World world){ m_World = world; }
	public World GetWorld(){ return m_World; }

	private void Awake()
	{
		m_stateMachine = GetComponent<StateMachine>();
	}
}
