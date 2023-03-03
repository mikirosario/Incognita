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
}
