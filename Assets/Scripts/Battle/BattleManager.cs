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
	private StringBuilder _currentBattleArea = new StringBuilder(20);
	//[SerializeField] private InputControllerBattleScene _inputController;
	private Character[] _playerParty = new Character[3];
	private GameObject BattleAreasObject { get { return _battleAreasObject; } set { _battleAreasObject = value; } }
	private GameObject BattleUIObject { get { return _battleUIObject; } set { _battleUIObject = value; } }
	private List<Spawnable> EnemySpawnPrefabs { get; set; }
	private List<Spawnable> PlayerSpawnPrefabs { get; set; }
	public Character[] PlayerParty { get { return _playerParty; } }
	public GameObject[] EnemyParty { get; private set; }
	public BattleAreaSelector BattleAreaSelector { get { return _battleAreaSelector; } }
	public StringBuilder CurrentBattleArea { get { return _currentBattleArea; } }
	//public InputControllerBattleScene InputController { get { return _inputController; } private set { _inputController = value; } }


	private void Awake()
	{
		for (int i = 0; i < PlayerParty.Length; ++i)
			PlayerParty[i] = null;
		//for (int i = 0; i < EnemyParty.Length; ++i)
		//	EnemyParty[i] = null;
		EnemySpawnPrefabs = new List<Spawnable>(3);
		PlayerSpawnPrefabs = new List<Spawnable>(3);

		//To test Battle Area, use these methods to initiate battle
		if (GameManager.Instance.SetActiveBattleScene) //<-Debug code
		{
			SetActiveBattleScene(true);
			LoadBattle();
		}
	}

	public void SetActiveBattleScene(bool doSet, string battleAreaName = null)
	{
		BattleAreasObject.SetActive(doSet);
		BattleUIObject.SetActive(doSet);
		if (doSet == false)
		{
			UnloadBattle();
			ClearPlayerParty();
		}
		else
		{
			foreach (GameObject player in GameManager.Instance.PlayerPartyPrefabs)
				player.GetComponent<Character>().SetBattleMode();
			LoadBattle(battleAreaName);
		}
	}
	public void SetDefaultBattleArea(string area)
	{
		if (BattleAreaSelector.BattleArea[area] != null)
		{
			CurrentBattleArea.Clear();
			CurrentBattleArea.Append(area);
		}
	}
	private void ClearPlayerParty()
	{
		for (int i = 0; i < PlayerParty.Length; ++i)
		{
			if (PlayerParty[i])
			{
				Destroy(PlayerParty[i].gameObject);
				PlayerParty[i] = null;
			}
		}
	}
	private void ClearEnemyParty()
	{
		for (int i = 0; i < EnemyParty.Length; ++i)
		{
			if (EnemyParty[i])
			{
				Destroy(EnemyParty[i].gameObject);
				EnemyParty[i] = null;
			}
		}
	}

	private void LoadBattle(string areaName = null)
	{
		if (areaName == null/* || BattleAreaSelector.BattleAreaObjects[areaName] == null*/) //could be better to allow areaName not found case to throw
			areaName = CurrentBattleArea.ToString();
		LoadArea(areaName);
		IBattleAreaController battleAreaController = BattleAreaSelector.GetBattleAreaController(areaName);
		if (battleAreaController == null || !GetEnemySpawnPrefabs(battleAreaController) || !GetPlayerSpawnPrefabs(battleAreaController))
			return; //return to previous scene with error log
		Spawn(PlayerSpawnPrefabs, EnemySpawnPrefabs);
		areaName = null;
		battleAreaController = null;
	}

	private void UnloadBattle()
	{
		UnloadArea(CurrentBattleArea.ToString());
	}

	private void LoadArea(string areaName)
	{
		UnloadArea(CurrentBattleArea.ToString());
		CurrentBattleArea.Clear();
		CurrentBattleArea.Append(areaName);
		BattleAreaSelector.GetBattleAreaGameObject(CurrentBattleArea.ToString()).SetActive(true);
		areaName = null;
	}

	private void UnloadArea(string areaName)
	{
		GameObject area = BattleAreaSelector.GetBattleAreaGameObject(areaName);
		if (area != null)
			area.SetActive(false);
		area = null;
	}

	private bool GetEnemySpawnPrefabs(IBattleAreaController area)
	{
		if (area.Enemies.Count < 1)
			return false;
		int enemyNumber = Random.Range(1, area.SpawnPointSelector.EnemyPositionMax + 1);
		for (int i = 0; enemyNumber > 0; ++i, --enemyNumber)
		{
			GameObject enemyPrefab = area.Enemies[Random.Range(0, area.Enemies.Count)]; //enemies currently selected at random, but eventually there should be predefined groups
			Transform spawnPosition = area.SpawnPointSelector.GetEnemyPosition(area.SpawnPointSelector.EnemyPositionKeys[i]);
			//EnemySpawnPrefabs[i] = new Spawnable(enemyPrefab, spawnPosition);
			EnemySpawnPrefabs.Add(new Spawnable(enemyPrefab, spawnPosition));
			enemyPrefab = null;
			spawnPosition = null;
		}
		return true;
	}

	private bool GetPlayerSpawnPrefabs(IBattleAreaController area)
	{
		int playerNumber = GameManager.Instance.PlayerPartyPrefabs.Count;
		if (playerNumber < 1)
			return false;
		for (int i = 0; playerNumber > 0; ++i, --playerNumber)
		{
			GameObject playerPrefab = GameManager.Instance.PlayerPartyPrefabs[i];
			Transform spawnPosition = area.SpawnPointSelector.GetPlayerPosition(area.SpawnPointSelector.PlayerPositionKeys[i]);
			PlayerSpawnPrefabs.Add(new Spawnable(playerPrefab, spawnPosition));
			playerPrefab = null;
			spawnPosition = null;
		}
		return true;
	}

	private void Spawn(List<Spawnable> playerSpawnPrefabs, List<Spawnable> enemySpawnPrefabs)
	{

		//PlayerParty[0] = (Instantiate(_playerPartyPrefabs[0], new Vector2(-4f, 0f), Quaternion.identity)).GetComponent<Character>();

		int i = 0;
		foreach (Spawnable player in playerSpawnPrefabs)
			PlayerParty[i++] = Instantiate(player.Prefab, player.SpawnPosition).GetComponent<Character>();
		foreach (Spawnable enemy in enemySpawnPrefabs)
			Instantiate(enemy.Prefab, enemy.SpawnPosition);
	}

	private class Spawnable
	{
		public GameObject Prefab { get; private set; }
		public Transform SpawnPosition { get; private set; }
		public Spawnable (GameObject enemyPrefab, Transform spawnPosition)
		{
			Prefab = enemyPrefab;
			SpawnPosition = spawnPosition;
		}
		~Spawnable()
		{
			Prefab = null;
		}
	}

}
