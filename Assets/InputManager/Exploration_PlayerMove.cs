using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Exploration_PlayerMove : MonoBehaviour
{
	private Vector2 movement;
	private Rigidbody2D rb;
	private Animator animator;
	[SerializeField] private float speed = 5f;
	private int animatorIsWalking;
	private int animatorX;
	private int animatorY;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		animatorIsWalking = Animator.StringToHash("isWalking");
		animatorX = Animator.StringToHash("X");
		animatorY = Animator.StringToHash("Y");
	}

	private void OnPlayerMove(InputValue input)
	{
		movement = input.Get<Vector2>();

		if (movement.x != 0 || movement.y != 0)
		{
			animator.SetBool(animatorIsWalking, true);
			animator.SetFloat(animatorX, movement.x);
			animator.SetFloat(animatorY, movement.y);
		}
		else
			animator.SetBool("isWalking", false);
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
	}
}
