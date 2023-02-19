using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
	public UIManager UIManager { get; private set; }
	public PlayerManager PlayerManager { get; private set; }
	public InputManager InputManager { get; private set; }
	public bool Paused { get { return InputManager.Common_Pause.Paused; } }

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
            Destroy(this);
            return;
		}
        Instance = this;
		UIManager = GameObject.Find("UI").GetComponent<UIManager>();
		PlayerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
		InputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
		Debug.Log(UIManager);
		Debug.Log(PlayerManager);
	}
}
