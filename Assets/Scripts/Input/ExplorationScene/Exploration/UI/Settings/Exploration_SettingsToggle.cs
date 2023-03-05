using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Exploration_SettingsToggle : MonoBehaviour, IMenuToggle
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private GameObject _settingsPanel;

	private GameObject SettingsPanel { get { return _settingsPanel; } set { _settingsPanel = value; } }
	private PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }
	public GameObject GameObject { get { return SettingsPanel; } }
	public IMenuToggle Next { get; set; }
	public IMenuToggle Prev { get; set; }
	public bool IsDisplayed { get; private set; }

	private void Awake()
	{
		PlayerInput.actions.FindActionMap("Exploration").FindAction("SettingsToggle").performed += OnSettingsToggle;
		List<IMenuToggle> menuList = new List<IMenuToggle>();
		IMenuToggle component;
		for (int i = 0; i < transform.childCount; ++i)
			if ((component = transform.GetChild(i).GetComponent<IMenuToggle>()) != null)
				menuList.Add(component);
		if (menuList.Count > 0)
			for (int pos = 0, mod = menuList.Count; pos < menuList.Count; ++pos)
			{
				menuList[pos].Next = menuList[(pos + 1) % mod];
				menuList[pos].Prev = menuList[(pos + menuList.Count - 1) % mod];
			}
		menuList.Clear();
		menuList = null;
	}

	public void Display(bool doDisplay)
	{
		SettingsPanel.SetActive(doDisplay);
	}

	private void OnSettingsToggle(InputAction.CallbackContext context)
	{
		GameManager.Instance.ExplorationManager.UIController.ToggleMenu(this);
	}


}
