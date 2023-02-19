using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Exploration_PlayerMove : MonoBehaviour
{
	[SerializeField] private Rigidbody2D _rb;
	[SerializeField] private Animator _animator;
	[SerializeField] private float _speed = 5f;
	private Rigidbody2D RigidBody { get { return _rb; } }
	private Animator Animator { get { return _animator; } }
	private float Speed { get { return _speed; } }
	private Vector2 Movement { get; set; }
	private int AnimatorVar_IsWalking { get; set; }
	private int AnimatorVar_X { get; set; }
	private int AnimatorVar_Y { get; set; }

	private void Awake()
	{
		AnimatorVar_IsWalking = Animator.StringToHash("isWalking");
		AnimatorVar_X = Animator.StringToHash("X");
		AnimatorVar_Y = Animator.StringToHash("Y");
	}

	public void UpdatePlayerInputs()
	{
		if (Movement.x != 0 || Movement.y != 0)
		{
			Animator.SetBool(AnimatorVar_IsWalking, true);
			Animator.SetFloat(AnimatorVar_X, Movement.x);
			Animator.SetFloat(AnimatorVar_Y, Movement.y);
		}
		else
			Animator.SetBool("isWalking", false);
	}

	private void OnPlayerMove(InputValue input)
	{
		Movement = input.Get<Vector2>();
		if (GameManager.Instance.Paused == false)
			UpdatePlayerInputs();
	}

	private void FixedUpdate()
	{
		RigidBody.MovePosition(RigidBody.position + Movement * Speed * Time.fixedDeltaTime);
	}
}
