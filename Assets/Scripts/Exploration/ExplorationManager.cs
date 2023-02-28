using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
	[SerializeField] private GameObject _explorationObject;
	[SerializeField] private UIManager _UIManager;
	[SerializeField] private PlayerManager _playerManager;
	[SerializeField] private InputController _inputController;
	private GameObject ExplorationObject { get { return _explorationObject; } set { _explorationObject = value; } }
	public UIManager UIManager { get { return _UIManager; } private set { _UIManager = value; } }
	public PlayerManager PlayerManager { get { return _playerManager; } private set { _playerManager = value; } }
	public InputController InputController { get { return _inputController; } private set { _inputController = value; } }

	private void Awake()
	{

		Debug.Log(UIManager);
		Debug.Log(PlayerManager);
	}

	public void SetActiveExplorationScene(bool isActive)
	{
		ExplorationObject.SetActive(isActive);
	}
}
