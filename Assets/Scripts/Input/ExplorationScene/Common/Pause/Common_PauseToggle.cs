using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Common_PauseToggle : MonoBehaviour
{
	private PlayerInput PlayerInput => GameManager.Instance.PlayerInput;
	private float UnpausedTimeScale { get; set; }
	private float ActiveTimeScale { get; set; }
	public bool Paused { get; private set; }
	//private InputManager InputManager => GameManager.Instance.InputManager;

	private void Awake()
	{
		UnpausedTimeScale = 1f;
		ActiveTimeScale = UnpausedTimeScale;
		Paused = false;
		PlayerInput.actions.FindActionMap("Common").FindAction("PauseToggle").performed += OnPauseToggle;
	}

	public void PauseGame(bool doPause)
	{
		if (Paused == doPause)
			return;
		Paused = doPause;
		if (Paused == false)
		{
			ActiveTimeScale = UnpausedTimeScale;
			Time.timeScale = ActiveTimeScale;
			if (GameManager.Instance.ActiveScene == GameManager.SceneIndex.ExplorationScene)
				GameManager.Instance.ExplorationManager.InputController.Exploration.PlayerMove.UpdatePlayerInputs();
		}
		else
		{
			UnpausedTimeScale = ActiveTimeScale;
			Time.timeScale = 0;
		}
	}
	private void OnPauseToggle(InputAction.CallbackContext context)
	{
		if (GameManager.Instance.ActiveScene != GameManager.SceneIndex.ExplorationScene || GameManager.Instance.ExplorationManager.UIController.IsMenuOpen == false) //Can't unpause while menu is displayed
			PauseGame(!Paused);
	}

	public void OnApplicationFocus(bool isInForeground)
	{
		if (isInForeground == false)
			PauseGame(true);
	}
}
