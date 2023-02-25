using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomRowController : MonoBehaviour
{
	[SerializeField] private PlayerSlotController _playerSlot0, _playerSlot1, _playerSlot2;

	public PlayerSlotController PlayerSlot0 { get { return _playerSlot0; } }
	public PlayerSlotController PlayerSlot1 { get { return _playerSlot1; } }
	public PlayerSlotController PlayerSlot2 { get { return _playerSlot2; } }
}
