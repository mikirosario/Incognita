using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerExplorationScene : MonoBehaviour
{
	public bool IsMenuOpen { get { return GameManager.Instance.ExplorationManager.InputController.Exploration.UIActive; } }
}
