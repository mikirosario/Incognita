using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
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
	public StringBuilder CurrentBattleArea { get; private set; }
	//public InputControllerBattleScene InputController { get { return _inputController; } private set { _inputController = value; } }


	private void Awake()
	{
		CurrentBattleArea = new StringBuilder("Town", 20);

		//To test Battle Area, use these methods to initiate battle
		if (GameManager.Instance.SetActiveBattleScene)
		{
			SetActiveBattleScene(true);
			LoadBattle();
		}
	}

	public void SetActiveBattleScene(bool doSet)
	{
		if (doSet == false)
			UnloadArea(CurrentBattleArea.ToString());
		BattleAreasObject.SetActive(doSet);
		BattleUIObject.SetActive(doSet);
	}

	private void LoadArea(string areaName)
	{
		UnloadArea(CurrentBattleArea.ToString());
		CurrentBattleArea.Clear();
		CurrentBattleArea.Append(areaName);
		BattleAreaSelector.GetBattleAreaGameObject(areaName).SetActive(true);
	}

	private void UnloadArea(string areaName)
	{
		BattleAreaSelector.GetBattleAreaGameObject(areaName).SetActive(false);
	}

	public void LoadBattle(string areaName = null)
	{
		if (areaName == null)
			areaName = CurrentBattleArea.ToString();
		LoadArea(areaName);
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
