using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerExplorationScene : MonoBehaviour
{
	[SerializeField] private GameObject _characterSheetToggle;
	[SerializeField] private GameObject _settingsToggle;
	private IMenuToggle _activeMenu = null;
	public bool IsMenuOpen { get { return GameManager.Instance.ExplorationManager.InputController.Exploration.UIActive; } }
	public GameObject CharacterSheetToggle { get { return _characterSheetToggle; } }
	public GameObject SettingsToggle { get { return _settingsToggle; } }
	private IMenuToggle ActiveMenu { get { return _activeMenu; } set { _activeMenu = value; } }

	public void ToggleMenu(IMenuToggle menu)
	{
		bool newDisplayState = !menu.GameObject.activeSelf;
		if (newDisplayState == true && GameManager.Instance.ExplorationManager.Paused == true && ActiveMenu == null) //Can't display menu while paused
			return;
		GameManager.Instance.ExplorationManager.InputController.Common.PauseToggle.PauseGame(newDisplayState); //Pause if opening menu
		bool menuIsSameAsActiveMenu = menu == ActiveMenu;
		if (ActiveMenu != null && !menuIsSameAsActiveMenu)
			CloseMenu(ActiveMenu);
		if (newDisplayState == true)
			OpenMenu(menu);
		else
			CloseMenu(menu);
	}

	private void OpenMenu(IMenuToggle menu)
	{
		menu.Display(true);
		ActiveMenu = menu;
	}

	private void CloseMenu(IMenuToggle menu)
	{
		menu.Display(false);
		ActiveMenu = null;
	}
}
