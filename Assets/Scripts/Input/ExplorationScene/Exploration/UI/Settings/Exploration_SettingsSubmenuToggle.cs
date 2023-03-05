using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploration_SettingsSubmenuToggle : MonoBehaviour, IMenuToggle
{
	[SerializeField] private GameObject _submenu;

	private GameObject Submenu { get { return _submenu; } set { _submenu = value; } }
	//private PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }
	public GameObject GameObject { get { return Submenu; } }
	public IMenuToggle Next { get; set; }
	public IMenuToggle Prev { get; set; }
	public bool IsDisplayed { get; private set; }

	public void Display(bool doDisplay)
	{
		Submenu.SetActive(doDisplay);
	}

	public void OnOptionToggle()
	{
		GameManager.Instance.ExplorationManager.UIController.SettingsController.ToggleSubMenu(this);
	}
}
