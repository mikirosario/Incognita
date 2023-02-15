using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
	[SerializeField] private GameObject[] playerParty;
	[SerializeField] private GameObject[] enemyParty;

	private void Start()
	{
		Spawn();
	}

	private void Spawn()
	{
		Instantiate(playerParty[0], new Vector2(-4f, 0f), Quaternion.identity);
		Instantiate(playerParty[0], new Vector2(2f, 0.5f), Quaternion.identity);
	}
}
