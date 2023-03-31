using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BattleManager : MonoBehaviour
{
	[SerializeField] private GameObject _battleAreasObject;
	[SerializeField] private GameObject _battleUIObject;
	[SerializeField] private BattleAreaSelector _battleAreaSelector;
	private StringBuilder _currentBattleArea = new StringBuilder(20);
	private List<Character> _playerParty = new List<Character>(3);
	private List<Character> _enemyParty = new List<Character>(6);
	private GameObject BattleAreasObject { get { return _battleAreasObject; } set { _battleAreasObject = value; } }
	private GameObject BattleUIObject { get { return _battleUIObject; } set { _battleUIObject = value; } }
	private List<Spawnable> EnemySpawnPrefabs { get; set; }
	private List<Spawnable> PlayerSpawnPrefabs { get; set; }
	public List<Character> PlayerParty { get { return _playerParty; } }
	public List<Character> EnemyParty { get { return _enemyParty; } }
	public BattleAreaSelector BattleAreaSelector { get { return _battleAreaSelector; } }
	public StringBuilder CurrentBattleArea { get { return _currentBattleArea; } }

	private void Awake()
	{
		EnemySpawnPrefabs = new List<Spawnable>(3);
		PlayerSpawnPrefabs = new List<Spawnable>(3);

		//To test Battle Area, use these methods to initiate battle
		if (GameManager.Instance.SetActiveBattleScene) //<-Debug code
		{
			SetActiveBattleScene(true);
			LoadBattle();
		}
	}

	public void AttackTarget()
	{
		Character selectedPlayer = PlayerParty[0];
		Character selectedEnemy = EnemyParty[0];
		Debug.Log("Attacking");
		StartCoroutine(selectedPlayer.AttackTarget(selectedEnemy));
	}

	public void SetActiveBattleScene(bool doSet, string battleAreaName = null)
	{
		BattleAreasObject.SetActive(doSet);
		BattleUIObject.SetActive(doSet);
		if (doSet == false)
			UnloadBattle();
		else
			LoadBattle(battleAreaName);
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
		for (int i = PlayerParty.Count; i > 0; --i)
		{
			Destroy(PlayerParty[0].gameObject);
			PlayerParty.RemoveAt(0);
		}
	}
	private void ClearEnemyParty()
	{
		for (int i = EnemyParty.Count; i > 0; --i)
		{
			Destroy(EnemyParty[0].gameObject);
			PlayerParty.RemoveAt(0);
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
		ClearPlayerParty();
		ClearEnemyParty();
		PlayerSpawnPrefabs.Clear();
		EnemySpawnPrefabs.Clear();
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
			enemyPrefab.GetComponent<Character>().SetBattleMode();
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
		foreach (Spawnable player in playerSpawnPrefabs)
		{
			PlayerParty.Add(Instantiate(player.Prefab, player.SpawnPosition).GetComponent<Character>());
			PlayerParty[PlayerParty.Count - 1].SetBattleMode();
		}
		foreach (Spawnable enemy in enemySpawnPrefabs)
		{
			EnemyParty.Add(Instantiate(enemy.Prefab, enemy.SpawnPosition).GetComponent<Character>());
			EnemyParty[EnemyParty.Count - 1].SetBattleMode();
		}
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
