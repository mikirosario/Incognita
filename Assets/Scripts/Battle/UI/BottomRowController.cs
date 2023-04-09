using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomRowController : MonoBehaviour
{
	//[SerializeField] private PlayerSlotController _playerSlot0, _playerSlot1, _playerSlot2;
	[SerializeField] private List<PlayerSlotController> _playerSlots = new List<PlayerSlotController>(3);
	public PlayerSlotController PlayerSlot0 { get { return _playerSlots[0]; } }
	public PlayerSlotController PlayerSlot1 { get { return _playerSlots[1]; } }
	public PlayerSlotController PlayerSlot2 { get { return _playerSlots[2]; } }

	public PlayerSlotController GetPlayerSlot(Character player)
	{
		PlayerSlotController ret = null;
		foreach (PlayerSlotController pscontroller in _playerSlots)
		{
			if (pscontroller.Character == player)
			{
				ret = pscontroller;
				break;
			}
		}
		return ret;
	}

	public void SetPlayerSlots(List<Character> playerParty)
	{
		for (int i = 0; i < _playerSlots.Count && i < playerParty.Count; ++i)
		{
			_playerSlots[i].gameObject.SetActive(true);
			_playerSlots[i].Character = playerParty[i];
		}
	}
}
