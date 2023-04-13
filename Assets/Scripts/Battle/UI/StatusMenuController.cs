using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMenuController : MonoBehaviour
{
	[SerializeField] private BottomRowController _bottomRowController;
	[SerializeField] private BattleActionPanelController _battleActionPanelController;
	[SerializeField] private TargetSelectorController _targetSelectorController;
	public BottomRowController BottomRowController { get { return _bottomRowController; } }
	public BattleActionPanelController BattleActionPanelController { get { return _battleActionPanelController; } }
	public TargetSelectorController TargetSelectorController { get { return _targetSelectorController; } }

	public void SetBattleMenu(List<Character> playerParty)
	{
		BottomRowController.SetPlayerSlots(playerParty);
	}
}
