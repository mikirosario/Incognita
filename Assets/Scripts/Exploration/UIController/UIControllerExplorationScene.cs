using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerExplorationScene : MonoBehaviour
{
	//[SerializeField] private GameObject _characterSheetToggle;
	//[SerializeField] private GameObject _settingsToggle;
	[SerializeField] private UISettingsSubmenuControllerExplorationScene _settingsSubmenuController;
	[SerializeField] private GameObject _menuSelectorsToggle;
	private IMenuToggle _activeMenu = null;
	public bool IsMenuOpen { get { return GameManager.Instance.ExplorationManager.InputController.Exploration.UIActive; } }
	//public GameObject CharacterSheetToggle { get { return _characterSheetToggle; } }
	//public GameObject SettingsToggle { get { return _settingsToggle; } }
	public GameObject MenuSelectorsToggle { get { return _menuSelectorsToggle; } }
	public UISettingsSubmenuControllerExplorationScene SettingsController { get { return _settingsSubmenuController; } }
	private IMenuToggle ActiveMenu { get { return _activeMenu; } set { _activeMenu = value; } }

	public void OpenNextMenu()
	{
		//if (ActiveMenu != ActiveMenu.Next) <- Need this if there is only 1 menu, otherwise superflous
		ToggleMenu(ActiveMenu.Next);
	}

	public void OpenPrevMenu()
	{
		//if (ActiveMenu != ActiveMenu.Prev)
		ToggleMenu(ActiveMenu.Prev);
	}

	public void ToggleMenu(IMenuToggle menu)
	{
		bool newDisplayState = !menu.GameObject.activeSelf;
		if (newDisplayState == true && GameManager.Instance.ExplorationManager.Paused == true && ActiveMenu == null) //Can't display menu on player-instigated pause
			return;
		GameManager.Instance.ExplorationManager.InputController.Common.PauseToggle.PauseGame(newDisplayState); //Game-instigated pause when opening menu, unpause when closing
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
		if (!MenuSelectorsToggle.activeSelf)
			MenuSelectorsToggle.SetActive(true);
	}

	private void CloseMenu(IMenuToggle menu)
	{
		menu.Display(false);
		ActiveMenu = null;
		if (MenuSelectorsToggle.activeSelf)
			MenuSelectorsToggle.SetActive(false);
	}
}
