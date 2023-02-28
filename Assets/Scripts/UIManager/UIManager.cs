using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public bool IsMenuOpen { get { return GameManager.Instance.ExplorationManager.InputController.Exploration.UIActive; /*GameManager.Instance.InputManager.Exploration.UIActive ACHIPAPI;*/ } }
}
