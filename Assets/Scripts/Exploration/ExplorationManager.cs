using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
	[SerializeField] private GameObject _explorationObject;
	[SerializeField] private UIControllerExplorationScene _UIControllerExplorationScene;
	[SerializeField] private PlayerManager _playerManager;
	[SerializeField] private InputControllerExplorationScene _inputController;
	private GameObject ExplorationObject { get { return _explorationObject; } set { _explorationObject = value; } }
	public UIControllerExplorationScene UIController { get { return _UIControllerExplorationScene; } private set { _UIControllerExplorationScene = value; } }
	public PlayerManager PlayerManager { get { return _playerManager; } private set { _playerManager = value; } }
	public InputControllerExplorationScene InputController { get { return _inputController; } private set { _inputController = value; } }
	public bool Paused { get { return InputController.Common.PauseToggle.Paused; } }

	private void Awake()
	{

		Debug.Log(UIController);
		Debug.Log(PlayerManager);
	}

	public void SetActiveExplorationScene(bool isActive)
	{
		ExplorationObject.SetActive(isActive);
	}
}
