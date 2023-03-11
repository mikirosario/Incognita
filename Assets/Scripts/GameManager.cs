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
	[SerializeField, Tooltip("When launching Playmode from Unity Editor, set this bool to go to battle scene")] bool _setActiveBattleScene = false; //<-Debug code
	[SerializeField] List<GameObject> _playableCharacterPrefabs;
	[SerializeField] ScreenTransitionController _screenTransitionController;
	private StringBuilder _defaultBattleArea = new StringBuilder(20);
	private List<GameObject> PlayableCharacterPrefabs { get { return _playableCharacterPrefabs; } }
	public static GameManager Instance { get; private set; }
	private SaveData SaveData { get; set; }
	private string InitialScene { get { return _initialScene; } }
	public bool SetActiveBattleScene { get { return _setActiveBattleScene; } } //<-Debug code
	private Scene[] LoadedScenes = new Scene[6];
	public ScreenTransitionController ScreenTransitionController { get { return _screenTransitionController; } private set { _screenTransitionController = value; } }
	public SceneIndex ActiveScene { get; set; }
	public BattleManager BattleManager { get; private set; }
	public ExplorationManager ExplorationManager { get; private set; }
	public List<GameObject> PlayerPartyPrefabs { get; private set; }

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
		PlayerPartyPrefabs = new List<GameObject>();
		//if save file, deserialize into SaveData, load data, SaveData = null.
		//else...		;
		PlayerPartyPrefabs.Add(PlayableCharacterPrefabs.Find(obj => obj.name.Equals("Kai")));
		//Debug.Log(PlayerPartyPrefabs[0].name);
	}

	private void Start()
	{
		StartCoroutine(LoadGameAsync());
	}

	IEnumerator LoadGameAsync() //load progress bar stuff will be referenced here
	{
		EditorLoad(); //<--replace with LoadGame() for production builds
		//LoadGame();
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
		SetActiveScene(SetActiveBattleScene == true ? SceneIndex.BattleScene : SceneIndex.ExplorationScene); //<--Editor check
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
	public void SetActiveScene(SceneIndex sceneIndex, string battleAreaName = null)
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
			BattleManager.SetActiveBattleScene(true, battleAreaName);
			ActiveScene = SceneIndex.BattleScene;
		}
		battleAreaName = null;
	}

	private IEnumerator TransitionActiveSceneAsync(IFadeOut fadeOut, IFadeIn fadeIn, SceneIndex sceneIndex, string battleAreaName)
	{
		StartCoroutine(fadeOut.Play());
		while (fadeOut.IsAnimating)
			yield return null;
		SetActiveScene(sceneIndex, battleAreaName);
		StartCoroutine(fadeIn.Play());
	}

	public void TransitionActiveScene(IFadeOut fadeOut, IFadeIn fadeIn, SceneIndex sceneIndex, string battleAreaName = null)
	{
		StartCoroutine(TransitionActiveSceneAsync(fadeOut, fadeIn, sceneIndex, battleAreaName));
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
