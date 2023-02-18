using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
	public UIManager UIManager { get; private set; }
	public PlayerManager PlayerManager { get; private set; }
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
		Debug.Log(UIManager);
		Debug.Log(PlayerManager);
	}

	private static void LoadGame()
	{

	}
}
