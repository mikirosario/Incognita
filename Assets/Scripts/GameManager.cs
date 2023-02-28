using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	public enum SceneIndex
	{
		GameManagerScene = 0,
		BattleScene = 1,
		ExplorationScene = 2,
		CutScene = 3,
		TitleScene = 4,
		EndingScene = 5
	}
	//every scene needs to set this, and update it as needed, so BattleManager knows what area assets to load
	private static StringBuilder _currentBattleArea = new StringBuilder("Town", 20);
	public static GameManager Instance { get; private set; }
	//private Dictionary<string, Scene> LoadedScenes = new Dictionary<string, Scene>(3);
	private Scene[] LoadedScenes = new Scene[6];
	public static string CurrentBattleArea { get { return _currentBattleArea.ToString(); } private set { _currentBattleArea.Clear(); _currentBattleArea.Append(value); } }
	public SceneIndex ActiveScene { get; set; }
	public BattleManager BattleManager { get; private set; }
	public ExplorationManager ExplorationManager { get; private set; }


	public UIManager UIManager { get; private set; }
	public PlayerManager PlayerManager { get; private set; }
	public InputManager InputManager { get; private set; }
	public bool Paused { get { return InputManager.CommonInputs.PauseToggle.Paused; } }
	private Scene GetLastLoadedScene()
	{
		return SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
	}
	private void RegisterScene(Scene scene, SceneIndex sceneIndex)
	{
		LoadedScenes[(int)sceneIndex] = scene;
		//LoadedScenes.Add(GetLastLoadedScene().name, GetLastLoadedScene());

	}

	private void SetActiveScene(SceneIndex sceneIndex)
	{
		if (sceneIndex == SceneIndex.ExplorationScene)
		{
			BattleManager.SetActiveBattleScene(false);
			ExplorationManager.SetActiveExplorationScene(true);
			ActiveScene = SceneIndex.ExplorationScene;
		}
		else
		{
			ExplorationManager.SetActiveExplorationScene(false);
			BattleManager.SetActiveBattleScene(true);
			ActiveScene = SceneIndex.BattleScene;
		}
	}



	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
            Destroy(this);
            return;
		}
        Instance = this;
		RegisterScene(GetLastLoadedScene(), SceneIndex.GameManagerScene);
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);//battle scene always loaded
		RegisterScene(GetLastLoadedScene(), SceneIndex.BattleScene);
		SceneManager.LoadScene("Hometown", LoadSceneMode.Additive); //load initial scene
		RegisterScene(GetLastLoadedScene(), SceneIndex.ExplorationScene);
	}


	private void RefreshExplorationManagerRef()
	{
		//GameObject[] rootGameObjects = LoadedScenes[(int)SceneIndex.ExplorationScene].GetRootGameObjects();
		//foreach (GameObject rootGameObject in rootGameObjects)
		//	if ((ExplorationManager = rootGameObject.GetComponent<ExplorationManager>()) != null)
		//		break;
		//rootGameObjects = null;
		ExplorationManager = GameObject.Find("ExplorationManager").GetComponent<ExplorationManager>(); // LIO PODIA HACER ASIIIII XXXXXXXXXXXOOOO
	}

	private void Start()
	{
		//Get GameManagerScene Refs
		InputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

		//Get Battle Scene Refs
		GameObject[] rootGameObjects = LoadedScenes[(int)SceneIndex.BattleScene].GetRootGameObjects();
		foreach (GameObject rootGameObject in rootGameObjects)
			if ((BattleManager = rootGameObject.GetComponent<BattleManager>()) != null)
				break;
		//Get Active Exploration Scene Ref for Initial Scene
		RefreshExplorationManagerRef(); //Integrate into Async loading schema using coroutine - Miki
		SetActiveScene(SceneIndex.ExplorationScene);
		//Get Active Exploration Scene Refs
		//ActiveScene.
		UIManager = GameObject.Find("UI").GetComponent<UIManager>();
		PlayerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

		Debug.Log(UIManager);
		Debug.Log(PlayerManager);
	}





	private void OnDestroy()
	{
		Instance = null;
	}
}
