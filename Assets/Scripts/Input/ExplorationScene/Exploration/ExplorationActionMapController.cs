using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationActionMapController : MonoBehaviour
{
	[SerializeField] private Exploration_PlayerMove _playerMove;
	[SerializeField] private Exploration_CharacterSheetToggle _characterSheetToggle;
	[SerializeField] private Exploration_SettingsToggle _settingsToggle;
	public bool UIActive { get { return CharacterSheetToggle.IsDisplayed | SettingsToggle.IsDisplayed; } }

	public Exploration_PlayerMove PlayerMove { get { return _playerMove; } }
	public Exploration_CharacterSheetToggle CharacterSheetToggle { get { return _characterSheetToggle; } }
	public Exploration_SettingsToggle SettingsToggle { get { return _settingsToggle; } }

	private void Awake()
	{
		//I made this unused function code to create a circular list of menu toggle options, for scrolling... I think? xD
		List<IMenuToggle> menuList = new List<IMenuToggle>();
		IMenuToggle component;
		for (int i = 0; i < transform.childCount; ++i)
			if ((component = transform.GetChild(i).GetComponent<IMenuToggle>()) != null)
				menuList.Add(component);
		component = null;
		if (menuList.Count > 0)
			for(int pos = 0, mod = menuList.Count; pos < menuList.Count; ++pos)
			{
				menuList[pos].Next = menuList[(pos + 1) % mod];
				menuList[pos].Prev = menuList[(pos + menuList.Count - 1) % mod];
			}
		menuList.Clear();
		menuList = null;
	}
}
