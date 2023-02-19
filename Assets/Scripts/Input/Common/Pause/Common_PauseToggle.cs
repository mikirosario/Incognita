using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Common_PauseToggle : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	private PlayerInput PlayerInput { get { return _playerInput; } }
	private float UnpausedTimeScale { get; set; }
	public bool Paused { get; private set; }
	private InputManager InputManager => GameManager.Instance.InputManager;

	private void Awake()
	{
		Paused = false;
		PlayerInput.actions.FindActionMap("Common").FindAction("PauseToggle").started += OnPauseToggle;
		PlayerInput.actions.FindActionMap("Common").FindAction("PauseToggle").performed += OnPauseToggle;
		PlayerInput.actions.FindActionMap("Common").FindAction("PauseToggle").canceled += OnPauseToggle;
	}

	public void PauseGame(bool doPause)
	{
		if (Paused == doPause)
			return;
		Paused = doPause;
		if (Paused == false)
		{
			Time.timeScale = UnpausedTimeScale;
			InputManager.Exploration.PlayerMove.UpdatePlayerInputs();
		}
		else
		{
			UnpausedTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}
	}
	private void OnPauseToggle(InputAction.CallbackContext context)
	{
		if (GameManager.Instance.UIManager.IsMenuOpen == false) //Can't unpause while menu is displayed
			PauseGame(!Paused);
	}

	public void OnApplicationFocus(bool isInForeground)
	{
		if (isInForeground == false)
			PauseGame(true);
	}
}
