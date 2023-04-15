using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControllerExplorationScene : MonoBehaviour
{
	[SerializeField] private ExplorationActionMapController _explorationActionMapController;
	[SerializeField] private CommonActionMapController _commonActionMapController;
	private PlayerInput PlayerInput => GameManager.Instance.PlayerInput;
	public ExplorationActionMapController Exploration { get { return _explorationActionMapController; } set { _explorationActionMapController = value; } }
	public CommonActionMapController Common { get { return _commonActionMapController; } set { _commonActionMapController = value; } }
	private void OnEnable()
	{
		PlayerInput.actions.FindActionMap("Exploration").Enable();
		PlayerInput.actions.FindActionMap("Common").Enable();
	}
	private void OnDisable()
	{
		PlayerInput.actions.FindActionMap("Exploration").Disable();
		PlayerInput.actions.FindActionMap("Common").Disable();
	}
}
