using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
	[SerializeField] private GameObject _explorationObject;
	[SerializeField] private UIControllerExplorationScene _UIControllerExplorationScene;
	[SerializeField] private PlayerController _playerController;
	[SerializeField] private InputControllerExplorationScene _inputController;
	[SerializeField] private string _battleArea;
	private GameObject ExplorationObject { get { return _explorationObject; } set { _explorationObject = value; } }
	public UIControllerExplorationScene UIController { get { return _UIControllerExplorationScene; } private set { _UIControllerExplorationScene = value; } }
	public PlayerController PlayerController { get { return _playerController; } private set { _playerController = value; } }
	public InputControllerExplorationScene InputController { get { return _inputController; } private set { _inputController = value; } }
	public string BattleArea { get { return _battleArea; } private set { _battleArea = value; } }
	public bool Paused { get { return InputController.Common.PauseToggle.Paused; } }

	private void Awake()
	{
		Debug.Log(UIController);
		Debug.Log(PlayerController);
	}
	public void SetActiveExplorationScene(bool doSet)
	{

		if (doSet == false)
			UnloadArea();
		else
		{
			LoadArea();
			if (PlayerController.Kai.gameObject.activeSelf)
				PlayerController.Kai.SetExplorationMode();
		}
	}
	private void LoadArea()
	{
		ExplorationObject.SetActive(true);
	}

	private void UnloadArea()
	{
		ExplorationObject.SetActive(false);
	}

}
