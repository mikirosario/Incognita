using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputExploration : MonoBehaviour
{
	[SerializeField] private Exploration_PlayerMove _playerMove;
	[SerializeField] private Exploration_CharacterSheetToggle _characterSheetToggle;
	public bool UIActive { get { return CharacterSheetToggle.IsDisplayed; } }

	public Exploration_PlayerMove PlayerMove { get { return _playerMove; } }
	public Exploration_CharacterSheetToggle CharacterSheetToggle { get { return _characterSheetToggle; } }
}
