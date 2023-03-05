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
		IMenuToggle[] menuArray = GetComponentsInChildren<IMenuToggle>();
		if (menuArray.Length > 0)
			for(int pos = 0, mod = menuArray.Length; pos < menuArray.Length; ++pos)
			{
				menuArray[pos].Next = menuArray[(pos + 1) % mod];
				menuArray[pos].Prev = menuArray[(pos + menuArray.Length - 1) % mod];
			}
		//Debug.Log(CharacterSheetToggle.Next);
		//Debug.Log(CharacterSheetToggle.Prev);
		//Debug.Log(SettingsToggle.Next);
		//Debug.Log(SettingsToggle.Prev);
	}
}
