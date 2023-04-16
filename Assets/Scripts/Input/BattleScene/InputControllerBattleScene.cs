using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControllerBattleScene : MonoBehaviour
{
	private PlayerInput PlayerInput => GameManager.Instance.PlayerInput;
	private void OnEnable()
	{
		PlayerInput.actions.FindActionMap("Battle").Enable();
		PlayerInput.actions.FindActionMap("Common").Enable();
	}
	private void OnDisable()
	{
		//When exiting, GameManager scene is unloaded first, so any references
		//to GameManager become null! O_O Please allow us to specify unloading
		//order, Unity devs.
		if (GameManager.Instance != null)
		{
			PlayerInput.actions.FindActionMap("Battle").Disable();
			PlayerInput.actions.FindActionMap("Common").Disable();
		}
	}
}
