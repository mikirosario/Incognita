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
	[SerializeField] string _initialScene = "Hometown";
	[SerializeField] bool _setActiveBattleScene = false;
	public static GameManager Instance { get; private set; }
	private string InitialScene { get { return _initialScene; } }
	private bool SetActiveBattleScene { get { return _setActiveBattleScene; } }
	private Scene[] LoadedScenes = new Scene[6];
	public SceneIndex ActiveScene { get; set; }
	public BattleManager BattleManager { get; private set; }
	public ExplorationManager ExplorationManager { get; private set; }

		//This is just for use by EditorLoad testing method.
		//Remove from production.
		private bool SceneContains<T>(Scene scene) //DEBUG CODE
		{
			GameObject[] sceneGameObjects = scene.GetRootGameObjects();
			foreach (GameObject gameObject in sceneGameObjects)
				if (gameObject.GetComponent<T>() != null)
					return true;
			return false;
		}
		//If more than one scene is additively loaded from editor, use the first two
		//additively loaded scenes, otherwise load as in build. This is here for
		//testing purposes only.
		private void EditorLoad() //DEBUG CODE
		{
			bool battleSceneLoaded = false;
			bool explorationSceneLoaded = false;
			bool gameManagerSceneLoaded = false;
			Scene scene;
			int i = 0;
			while (i < SceneManager.sceneCount && i < 3)
			{
				scene = SceneManager.GetSceneAt(i);
				if (scene.name == "BattleScene" && battleSceneLoaded == false)
				{
					battleSceneLoaded = true;
					RegisterScene(scene, SceneIndex.BattleScene);
				}
				else if (SceneContains<ExplorationManager>(scene) && explorationSceneLoaded == false)
				{
					explorationSceneLoaded = true;
					RegisterScene(scene, SceneIndex.ExplorationScene);
				}
				else if (SceneContains<GameManager>(scene) && gameManagerSceneLoaded == false)
				{
					gameManagerSceneLoaded = true;
					RegisterScene(scene, SceneIndex.GameManagerScene);
				}
				++i;
			}
			if (gameManagerSceneLoaded == false)
			{
				SceneManager.LoadScene(0);
				RegisterScene(GetLastLoadedScene(), SceneIndex.GameManagerScene);
			}
			if (battleSceneLoaded == false)
			{
				SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);//battle scene always loaded
				RegisterScene(GetLastLoadedScene(), SceneIndex.BattleScene);
			}
			if (explorationSceneLoaded == false)
			{
				SceneManager.LoadScene("Hometown", LoadSceneMode.Additive); //load initial scene
				RegisterScene(GetLastLoadedScene(), SceneIndex.ExplorationScene);
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
		StartCoroutine(LoadGameAsync());
	}

	IEnumerator LoadGameAsync() //load progress bar stuff will be referenced here
	{
		EditorLoad(); //<--replace with LoadGame() for production builds
		BitArray loadConfirmation = new BitArray(3);
		bool isFullyLoaded = false;
		while (isFullyLoaded == false)
		{
			yield return null;
			for (int i = 0; i < 3; ++i)
				if (LoadedScenes[i].isLoaded == true)
					loadConfirmation.Set(i, true);
			isFullyLoaded = loadConfirmation.Get(0) & loadConfirmation.Get(1) & loadConfirmation.Get(2);
		}
		//Get Battle Scene Ref
		BattleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
		//Get Active Exploration Scene Ref for Initial Scene
		RefreshExplorationManagerRef();
		SetActiveScene(SetActiveBattleScene == true ? SceneIndex.BattleScene : SceneIndex.ExplorationScene);
		//SetActiveScene(SceneIndex.ExplorationScene);
	}

	private void LoadGame()
	{
		RegisterScene(GetLastLoadedScene(), SceneIndex.GameManagerScene);
		SceneManager.LoadScene("BattleScene", LoadSceneMode.Additive);//battle scene always loaded
		RegisterScene(GetLastLoadedScene(), SceneIndex.BattleScene);
		SceneManager.LoadScene(InitialScene, LoadSceneMode.Additive); //load initial scene
		RegisterScene(GetLastLoadedScene(), SceneIndex.ExplorationScene);
	}

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
	private void RefreshExplorationManagerRef()
	{
		ExplorationManager = GameObject.Find("ExplorationManager").GetComponent<ExplorationManager>();
	}
	private void OnDestroy()
	{
		Instance = null;
	}
}
