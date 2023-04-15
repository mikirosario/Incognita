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
		PlayerInput.actions.FindActionMap("Battle").Disable();
		PlayerInput.actions.FindActionMap("Common").Disable();
	}
}
