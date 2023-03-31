using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputSystemExt
{
	public static List<InputAction> DisableInputs()
	{
		List<InputAction> actionList = InputSystem.ListEnabledActions();
		InputSystem.DisableAllEnabledActions();
		return actionList;
	}

	public static void EnableInputs(List<InputAction> actionList)
	{
		foreach (InputAction action in actionList)
			action.Enable();
	}
}
