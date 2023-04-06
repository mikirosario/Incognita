using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIController : MonoBehaviour
{
	[SerializeField] private StatusMenuController _statusMenuController;

	public StatusMenuController StatusMenuController { get { return _statusMenuController; } }
}
