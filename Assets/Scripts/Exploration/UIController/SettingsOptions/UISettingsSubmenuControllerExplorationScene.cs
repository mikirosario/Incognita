using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingsSubmenuControllerExplorationScene : MonoBehaviour
{
	[SerializeField] private GameObject _languageSubMenuToggle;
	private IMenuToggle _activeMenu = null;
	public bool IsMenuOpen { get { return GameManager.Instance.ExplorationManager.InputController.Exploration.UIActive; } }
	public GameObject LanguageSubMenuToggle { get { return _languageSubMenuToggle; } }
	private IMenuToggle ActiveMenu { get { return _activeMenu; } set { _activeMenu = value; } }

	public void OpenNextMenu()
	{
		if (ActiveMenu != ActiveMenu.Next) //<- Need this if there is only 1 menu, otherwise superflous
			ToggleSubMenu(ActiveMenu.Next);
	}

	public void OpenPrevMenu()
	{
		if (ActiveMenu != ActiveMenu.Prev)
			ToggleSubMenu(ActiveMenu.Prev);
	}

	public void ToggleSubMenu(IMenuToggle menu)
	{
		bool newDisplayState = !menu.GameObject.activeSelf;
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
