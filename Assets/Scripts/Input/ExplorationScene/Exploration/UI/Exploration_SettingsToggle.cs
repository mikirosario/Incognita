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
	public bool IsDisplayed { get; private set; }

	private void Awake()
	{
		PlayerInput.actions.FindActionMap("Exploration").FindAction("SettingsToggle").performed += OnSettingsToggle;
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
