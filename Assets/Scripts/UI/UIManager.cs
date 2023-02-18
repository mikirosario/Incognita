using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField] private ExplorationMenuController _explorationMenu;

	private ExplorationMenuController ExplorationMenu { get { return _explorationMenu; } }
}
