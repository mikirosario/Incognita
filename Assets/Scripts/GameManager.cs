using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
	//every scene needs to set this, and update it as needed, so BattleManager knows what area assets to load
	private static StringBuilder _currentBattleArea = new StringBuilder("Town", 20);
	private static Color _namiColor = new Color(1f, 0.92f, 0.016f);
	private static Color _marlinColor = new Color(1f, 0.4549f, 0f); //send these to scriptables??
	private static Color _kaiColor = new Color(0.6084906f, 0.6560858f, 1f);
	public static GameManager Instance { get; private set; }
	public static string CurrentBattleArea { get { return _currentBattleArea.ToString(); } private set { _currentBattleArea.Clear(); _currentBattleArea.Append(value); } }
	public UIManager UIManager { get; private set; }
	public PlayerManager PlayerManager { get; private set; }
	public InputManager InputManager { get; private set; }
	public bool Paused { get { return InputManager.Common.PauseToggle.Paused; } }

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
