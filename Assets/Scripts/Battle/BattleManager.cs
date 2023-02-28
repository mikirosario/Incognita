using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Area = IBattleAreaController.Area;

public class BattleManager : MonoBehaviour
{
	[SerializeField] private GameObject _battleAreasObject;
	[SerializeField] private GameObject _battleUIObject;
	[SerializeField] private GameObject[] _playerPartyPrefabs;
	[SerializeField] private BattleAreaSelector _battleAreaSelector;
	//[SerializeField] private InputControllerBattleScene _inputController;
	private Character[] _playerParty = new Character[3];

	private GameObject BattleAreasObject { get { return _battleAreasObject; } set { _battleAreasObject = value; } }
	private GameObject BattleUIObject { get { return _battleUIObject; } set { _battleUIObject = value; } }
	private EnemySpawn[] EnemySpawnPrefabs { get; set; }
	private GameObject[] PlayerPartyPrefabs { get; set; }
	public Character[] PlayerParty { get { return _playerParty; } }
	public GameObject[] EnemyParty { get; private set; }
	public BattleAreaSelector BattleAreaSelector { get { return _battleAreaSelector; } }
	//public InputControllerBattleScene InputController { get { return _inputController; } private set { _inputController = value; } }


	private void Start()
	{
		//BattleAreasObject.SetActive(true);
		//BattleUIObject.SetActive(true);
		//LoadBattle();
	}

	public void SetActiveBattleScene(bool isActive)
	{
		BattleAreasObject.SetActive(isActive);
		BattleUIObject.SetActive(isActive);
	}

//	private void LoadArea(IBattleAreaController)

	public void LoadBattle(string areaName = null)
	{
		if (areaName == null)
			areaName = GameManager.CurrentBattleArea;
		BattleAreaSelector.GetBattleAreaGameObject(areaName).SetActive(true);
		IBattleAreaController battleAreaController = BattleAreaSelector.GetBattleAreaController(areaName);		
		if (battleAreaController == null || !GetEnemySpawnPrefabs(battleAreaController))
			return; //return to previous scene with error log
		Spawn(EnemySpawnPrefabs);
	}



	private bool GetEnemySpawnPrefabs(IBattleAreaController area)
	{
		if (area.Enemies.Count < 1)
			return false;
		int enemyNumber = Random.Range(1, area.SpawnPointSelector.EnemyPositionMax + 1);
		EnemySpawnPrefabs = new EnemySpawn[enemyNumber];
		for (int i = 0; enemyNumber > 0; ++i, --enemyNumber)
		{
			GameObject enemyPrefab = area.Enemies[Random.Range(0, area.Enemies.Count)]; //enemies currently selected at random, but eventually there should be predefined groups
			Transform spawnPosition = area.SpawnPointSelector.GetEnemyPosition(area.SpawnPointSelector.EnemyPositionKeys[i]);
			EnemySpawnPrefabs[i] = new EnemySpawn(enemyPrefab, spawnPosition);
		}
		return true;
	}

	private void Spawn(/*GameObject[] playerParty, */EnemySpawn[] enemySpawnPrefabs)
	{
		PlayerParty[0] = (Instantiate(_playerPartyPrefabs[0], new Vector2(-4f, 0f), Quaternion.identity)).GetComponent<Character>();
		PlayerParty[0].SetBattleMode();
		foreach (EnemySpawn enemy in enemySpawnPrefabs)
			Instantiate(enemy.Prefab, enemy.SpawnPosition);
	}

	private class EnemySpawn
	{
		public GameObject Prefab { get; private set; }
		public Transform SpawnPosition { get; private set; }
		public EnemySpawn (GameObject enemyPrefab, Transform spawnPosition)
		{
			Prefab = enemyPrefab;
			SpawnPosition = spawnPosition;
		}
	}

}
