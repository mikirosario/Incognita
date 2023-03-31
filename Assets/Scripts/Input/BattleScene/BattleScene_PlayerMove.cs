using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BattleScene_PlayerMove : MonoBehaviour
{
	//[SerializeField] private PlayerInput _playerInput; ////maybe
	[SerializeField] private Rigidbody2D _rb;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _speed = 5f;

	public void DoAttack(Character attacker, Character target)
	{

	}
}
