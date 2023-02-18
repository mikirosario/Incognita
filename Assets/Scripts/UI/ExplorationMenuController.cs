using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationMenuController : MonoBehaviour
{
	[SerializeField] private CharacterSheetController _characterSheetController;

	public CharacterSheetController CharacterSheet { get { return _characterSheetController; } }
}
