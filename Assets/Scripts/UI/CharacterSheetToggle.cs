using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSheetToggle : MonoBehaviour
{
	[SerializeField] private GameObject _characterSheetPanel;
	public GameObject CharacterSheetPanel { get { return _characterSheetPanel; } }
	private void OnCharacterSheetToggle()
	{
		Debug.Log("toggle");
		CharacterSheetPanel.SetActive(!CharacterSheetPanel.activeSelf);
	}
}
