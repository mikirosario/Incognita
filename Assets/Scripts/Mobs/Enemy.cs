using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Character, IMob
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
			GameManager.Instance.SetActiveScene(GameManager.SceneIndex.BattleScene, GameManager.Instance.ExplorationManager.BattleArea);
	}
}
