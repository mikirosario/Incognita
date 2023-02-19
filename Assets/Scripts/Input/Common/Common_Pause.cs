using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common_Pause : MonoBehaviour
{
	private float UnpausedTimeScale { get; set; }
	public bool Paused { get; private set; }
	private InputManager InputManager => GameManager.Instance.InputManager;

	private void Awake()
	{
		Paused = false;
	}

	public void PauseGame(bool doPause)
	{
		if (Paused == doPause)
			return;
		Paused = doPause;
		if (Paused == false)
		{
			Time.timeScale = UnpausedTimeScale;
			InputManager.Exploration_PlayerMove.UpdatePlayerInputs();
		}
		else
		{
			UnpausedTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}
	}
	private void OnPauseToggle()
	{
		PauseGame(!Paused);
	}

	public void OnApplicationFocus(bool isInForeground)
	{
		if (isInForeground == false)
			PauseGame(true);
	}
}
