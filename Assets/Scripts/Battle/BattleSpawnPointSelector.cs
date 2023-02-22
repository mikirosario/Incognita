using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BattleSpawnPointSelector : MonoBehaviour
{
	[SerializeField] private GameObject _enemySpawnPoints;
	[SerializeField] private GameObject _playerSpawnPoints;

	//[SerializeField] private List<BattleEnemyPosition> _enemyPositions;
	//[SerializeField] private List<BattlePlayerPosition> _playerPositions;

	//public List<BattleEnemyPosition> EnemyPositions { get { return _enemyPositions; } }
	//public List<BattlePlayerPosition> PlayerPositions { get { return _playerPositions; } }
	public Dictionary<string, Transform> EnemyPositions { get; private set; }
	public List<string> EnemyPositionKeys { get; private set; }
	public Dictionary<string, Transform> PlayerPositions { get; private set; }
	public List<string> PlayerPositionKeys { get; private set; }
	//public List<BattlePlayerPosition> PlayerPositions { get { return _playerPositions; } }
	public int EnemyPositionMax { get { return EnemyPositions.Count; } }

	private void Awake()
	{
		BattleEnemyPosition[] battleEnemyPositions = _enemySpawnPoints.GetComponentsInChildren<BattleEnemyPosition>();
		EnemyPositions = new Dictionary<string, Transform>(battleEnemyPositions.Length);
		foreach (BattleEnemyPosition battleEnemyPosition in battleEnemyPositions)
			EnemyPositions.Add(battleEnemyPosition.Name, battleEnemyPosition.Transform);
		BattlePlayerPosition[] battlePlayerPositions = _playerSpawnPoints.GetComponentsInChildren<BattlePlayerPosition>();
		PlayerPositions = new Dictionary<string, Transform>(battlePlayerPositions.Length);
		foreach (BattlePlayerPosition battlePlayerPosition in battlePlayerPositions)
			PlayerPositions.Add(battlePlayerPosition.Name, battlePlayerPosition.Transform);
		EnemyPositionKeys = new List<string>(EnemyPositions.Keys);
		PlayerPositionKeys = new List<string>(PlayerPositions.Keys);
	}
	public Transform GetEnemyPosition(string positionName)
	{
		return EnemyPositions[positionName];
	}

	public Transform GetPlayerPosition(string positionName)
	{
		return PlayerPositions[positionName];
	}
	public interface IPosition : IEquatable<IPosition>
	{
		public Transform Transform { get; }
		public string Name { get; }
	}
}
