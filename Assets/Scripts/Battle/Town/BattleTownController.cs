using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTownController : MonoBehaviour, IBattleAreaController
{
	//[SerializeField] private IBattleAreaController.Area _areaKey = IBattleAreaController.Area.Town;
	[SerializeField] private List<GameObject> _enemyList;
	[SerializeField] private BattleSpawnPointSelector _spawnPointSelector;
	public string Name { get { return gameObject.name; } }
	//public IBattleAreaController.Area AreaKey { get { return _areaKey; } }
	public List<GameObject> Enemies { get { return _enemyList; } }
	public BattleSpawnPointSelector SpawnPointSelector { get { return _spawnPointSelector; } }
}
