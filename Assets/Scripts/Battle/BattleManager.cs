using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BattleManager : MonoBehaviour
{
	public enum IAmError
	{
		None = 0,
		AreaNotFound,
		AreaControllerNotFound,
		EnemySpawnsNotFound,
		PlayerSpawnsNotFound
	}
	[SerializeField] private BattleUIController _battleUIController;
	[SerializeField] private GameObject _battleObject;
	[SerializeField] private BattleAreaSelector _battleAreaSelector;
	[SerializeField, ReadOnly] private IAmError _error = IAmError.None;
	private StringBuilder _currentBattleArea = new StringBuilder(20);
	private List<Character> _playerParty = new List<Character>(3);
	private List<Character> _enemyParty = new List<Character>(6);
	private List<Character> _turnOrder = new List<Character>(9);//establish turn order by Evasion?
	private List<Character> _verticalPositionOrder = new List<Character>(9);
	private int _turnOrderIndex;
	private GameObject BattleObject { get { return _battleObject; } }
	private List<Spawnable> EnemySpawnPrefabs { get; set; }
	private List<Spawnable> PlayerSpawnPrefabs { get; set; }
	private Character CurrentSelected { get; set; }
	private Character NextCharacter { get { return TurnOrder[TurnOrderIndex++]; } }
	private int TurnOrderIndex { get { return _turnOrderIndex; } set { _turnOrderIndex = value % TurnOrder.Count; } }
	public Character CurrentCharacter { get; private set; }
	public List<Character> PlayerParty { get { return _playerParty; } }
	public List<Character> EnemyParty { get { return _enemyParty; } }
	public List<Character> TurnOrder { get { return _turnOrder; } private set { _turnOrder = value; } }
	public List<Character> VerticalPositionOrder { get { return _verticalPositionOrder; } }
	public BattleAreaSelector BattleAreaSelector { get { return _battleAreaSelector; } }
	public StringBuilder CurrentBattleArea { get { return _currentBattleArea; } }
	public BattleUIController BattleUIController { get { return _battleUIController; } }
	public bool IsBattleOver { get; set; } = false;
	public IAmError Error { get { return _error; } set { _error = value; } }

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

	private void DisplayError(string funcName)
	{
		StringBuilder errorLog = new StringBuilder(funcName, 50);
		errorLog.Append(": ");
		switch (Error)
		{
			case IAmError.AreaNotFound:
				errorLog.Append("Area Not Found");
				break;
			case IAmError.AreaControllerNotFound:
				errorLog.Append("Area Controller Not Found");
				break;
			case IAmError.EnemySpawnsNotFound:
				errorLog.Append("Enemy Spawns Not Found");
				break;
			case IAmError.PlayerSpawnsNotFound:
				errorLog.Append("Player Spawns Not Found");
				break;
			default:
				errorLog.Append("No Error Logged");
				break;
		}
		Debug.LogWarning(errorLog); //replace with popup window
	}

	private void NextTurn()
	{
		CurrentCharacter = NextCharacter;
		if (PlayerParty.Contains(CurrentCharacter))
		{
			PlayerSlotController psc = BattleUIController.StatusMenuController.BottomRowController.GetPlayerSlot(CurrentCharacter);
			//ActionPanel setup here, or in slide?
			BattleUIController.StatusMenuController.BattleActionPanelController.ActionPanelSlide(psc); //Slide out
		}
	}

	public void ToggleSelectTarget()
	{
		if (!BattleUIController.StatusMenuController.TargetSelectorController.IsSelectorActive)
			BattleUIController.StatusMenuController.TargetSelectorController.StartSelector(TargetSelectorController.Selectable.LIVE_ENEMIES);
		else
			BattleUIController.StatusMenuController.TargetSelectorController.StopSelector();
	}

	public void AttackTarget()
	{
		Character selectedPlayer = CurrentCharacter;
		Character selectedEnemy = EnemyParty[0]; //character select functionality
		PlayerSlotController psc = BattleUIController.StatusMenuController.BottomRowController.GetPlayerSlot(CurrentCharacter);
		Debug.Log("Attacking");
		BattleUIController.StatusMenuController.BattleActionPanelController.ActionPanelSlide(psc); //Slide in
		StartCoroutine(selectedPlayer.AttackTarget(selectedEnemy));
		//NextTurn() //Maybe subscribe this to Action or naa?
	}

	//Always call this first
	public void SetActiveBattleScene(bool doSet, string battleAreaName = null)
	{
		BattleObject.SetActive(true);
		if (doSet == false)
			UnloadBattle();
		else
			LoadBattle(battleAreaName);
		BattleObject.SetActive(doSet);
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

	private void SetTurnOrder() //Will use Evasion (rename to Dexterity?) to establish turn order
	{
		TurnOrder.Clear();
		foreach (Character enemy in EnemyParty)
			TurnOrder.Add(enemy);
		foreach (Character player in PlayerParty)
			TurnOrder.Add(player);
		TurnOrder.Sort(Comparer<Character>.Create((a, b) => b.Evasion.Attribute.CompareTo(a.Evasion.Attribute)));
		TurnOrderIndex = 0;
	}

	private void SetVerticalPositionOrder()
	{
		VerticalPositionOrder.Clear();
		foreach (Character enemy in EnemyParty)
			VerticalPositionOrder.Add(enemy);
		foreach (Character player in PlayerParty)
			VerticalPositionOrder.Add(player);
		VerticalPositionOrder.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
	}

	//Always call this before battle
	private void LoadBattle(string areaName = null)
	{
		if (areaName == null)
			areaName = CurrentBattleArea.ToString();
		LoadArea(areaName);
		IBattleAreaController battleAreaController;
		if (!LoadArea(areaName)
			|| (battleAreaController = BattleAreaSelector.GetBattleAreaController(areaName)) == null
			|| !GetEnemySpawnPrefabs(battleAreaController)
			|| !GetPlayerSpawnPrefabs(battleAreaController))
		{
			DisplayError("LoadBattle");
			GameManager.Instance.SetActiveScene(GameManager.SceneIndex.ExplorationScene); //return to previous exploration scene with error log
			return;
		}
		Spawn(PlayerSpawnPrefabs, EnemySpawnPrefabs);
		SetTurnOrder();
		SetVerticalPositionOrder();
		BattleUIController.StatusMenuController.SetBattleMenu(PlayerParty);
		NextTurn();
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

	private bool LoadArea(string areaName)
	{
		GameObject newArea = BattleAreaSelector.GetBattleAreaGameObject(areaName);
		if (newArea != null)
		{
			UnloadArea(CurrentBattleArea.ToString());
			CurrentBattleArea.Clear();
			CurrentBattleArea.Append(areaName);
			newArea.SetActive(true);
		}
		areaName = null;
		newArea = null;	
		return Error == IAmError.None ? true : false;
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
		{
			Error = IAmError.EnemySpawnsNotFound;
			return false;
		}
		int enemyNumber = Random.Range(1, area.SpawnPointSelector.EnemyPositionMax + 1);
		for (int i = 0; enemyNumber > 0; ++i, --enemyNumber)
		{
			GameObject enemyPrefab = area.Enemies[Random.Range(0, area.Enemies.Count)]; //enemies currently selected at random, but eventually there should be predefined groups
			Transform spawnPosition = area.SpawnPointSelector.GetEnemyPosition(area.SpawnPointSelector.EnemyPositionKeys[i]);
			enemyPrefab.GetComponent<Character>().SetBattleMode();
			EnemySpawnPrefabs.Add(new Spawnable(enemyPrefab, spawnPosition));
			enemyPrefab = null;
			spawnPosition = null;
		}
		Error = IAmError.None;
		return true;
	}

	private bool GetPlayerSpawnPrefabs(IBattleAreaController area)
	{
		int playerNumber = GameManager.Instance.PlayerPartyPrefabs.Count;
		if (playerNumber < 1)
		{
			Error = IAmError.PlayerSpawnsNotFound;
			return false;
		}
		for (int i = 0; playerNumber > 0; ++i, --playerNumber)
		{
			GameObject playerPrefab = GameManager.Instance.PlayerPartyPrefabs[i];
			Transform spawnPosition = area.SpawnPointSelector.GetPlayerPosition(area.SpawnPointSelector.PlayerPositionKeys[i]);
			PlayerSpawnPrefabs.Add(new Spawnable(playerPrefab, spawnPosition));
			playerPrefab = null;
			spawnPosition = null;
		}
		Error = IAmError.None;
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
