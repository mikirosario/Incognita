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
	private Scene GetLastLoadedScene()
	{
		return SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
	}
	private void RegisterScene(Scene scene, SceneIndex sceneIndex)
	{
		LoadedScenes[(int)sceneIndex] = scene;
	}

	//Set which LoadedScene is the active scene.
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
		ExplorationManager = GameObject.Find("ExplorationManager").GetComponent<ExplorationManager>();
	}

	private void Start()
	{
		//Get GameManagerScene Refs

		//Get Battle Scene Ref
		BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
		//Get Active Exploration Scene Ref for Initial Scene
		RefreshExplorationManagerRef(); //Integrate into Async loading schema using coroutine - Miki
		SetActiveScene(SceneIndex.ExplorationScene);
	}





	private void OnDestroy()
	{
		Instance = null;
	}
}
