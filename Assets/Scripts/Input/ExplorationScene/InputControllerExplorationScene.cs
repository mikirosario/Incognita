using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControllerExplorationScene : MonoBehaviour
{
	[SerializeField] private ExplorationActionMapController _explorationActionMapController;
	[SerializeField] private CommonActionMapController _commonActionMapController;
	[SerializeField] private PlayerInput _playerInput;
	public ExplorationActionMapController Exploration { get { return _explorationActionMapController; } set { _explorationActionMapController = value; } }
	public CommonActionMapController Common { get { return _commonActionMapController; } set { _commonActionMapController = value; } }
	public PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }

	private void Awake()
	{
		PlayerInput.actions.FindActionMap("Exploration").Enable();
		PlayerInput.actions.FindActionMap("Common").Enable();
	}
}
