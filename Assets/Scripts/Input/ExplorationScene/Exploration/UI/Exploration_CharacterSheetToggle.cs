using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Exploration_CharacterSheetToggle : MonoBehaviour
{
	[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private GameObject _characterSheetPanel;
	[SerializeField] private TextMeshProUGUI	_displayedAttackValue, _displayedDefenceValue,
												_displayedShieldValue, _displayedHitChanceValue,
												_displayedEvasionValue, _displayedHitPoints,
												_displayedResonancePoints;
	//[SerializeField] private PlayerInput _playerInput;
	private StringBuilder _stringBuilder = new StringBuilder(20);
	private PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }
	private PlayerManager PlayerManager => GameManager.Instance.PlayerManager;
	private TextMeshProUGUI DisplayedAttackValue { get { return _displayedAttackValue; } }
	private TextMeshProUGUI DisplayedDefenceValue { get { return _displayedDefenceValue; } }
	private TextMeshProUGUI DisplayedShieldValue { get { return _displayedShieldValue; } }
	private TextMeshProUGUI DisplayedHitChanceValue { get { return _displayedHitChanceValue; } }
	private TextMeshProUGUI DisplayedEvasionValue { get { return _displayedEvasionValue; } }
	private TextMeshProUGUI DisplayedHitPoints { get { return _displayedHitPoints; } }
	private TextMeshProUGUI DisplayedResonancePoints { get { return _displayedResonancePoints; } }
	public GameObject CharacterSheetPanel { get { return _characterSheetPanel; } }
	public bool IsDisplayed { get; private set; }
	private void Start()
	{
		PlayerInput.actions.FindActionMap("Exploration").FindAction("CharacterSheetToggle").started += OnCharacterSheetToggle;
		PlayerInput.actions.FindActionMap("Exploration").FindAction("CharacterSheetToggle").performed += OnCharacterSheetToggle;
		PlayerInput.actions.FindActionMap("Exploration").FindAction("CharacterSheetToggle").canceled += OnCharacterSheetToggle;
	}

	public void DisplayCharacterSheet(bool doDisplay)
	{
		if (doDisplay == true && GameManager.Instance.ExplorationManager.Paused == true) //Can't display menu while paused
			return;
		IsDisplayed = doDisplay;
		GameManager.Instance.ExplorationManager.InputController.InputCommon.PauseToggle.PauseGame(doDisplay);
		//GameManager.Instance.InputManager.Common.PauseToggle.PauseGame(doDisplay); ACHIPAPI
		DisplayedAttackValue.text = BuildValueString(PlayerManager.Kai.Attack.Attribute);
		DisplayedDefenceValue.text = BuildValueString(PlayerManager.Kai.Defence.Attribute);
		DisplayedShieldValue.text = BuildValueString(PlayerManager.Kai.Shield.Attribute);
		DisplayedHitChanceValue.text = BuildValueString(PlayerManager.Kai.HitChance.Attribute);
		DisplayedEvasionValue.text = BuildValueString(PlayerManager.Kai.Evasion.Attribute);
		DisplayedHitPoints.text = BuildRangeString(PlayerManager.Kai.HitPointsCurrent, PlayerManager.Kai.HitPointsMax);
		DisplayedResonancePoints.text = BuildRangeString(PlayerManager.Kai.ResonancePointsCurrent, PlayerManager.Kai.ResonancePointsMax);
		CharacterSheetPanel.SetActive(doDisplay);
	}
	private void OnCharacterSheetToggle(InputAction.CallbackContext context)
	{
		DisplayCharacterSheet(!CharacterSheetPanel.activeSelf);
	}
	private string BuildValueString(uint value)
	{
		_stringBuilder.Clear();
		_stringBuilder.Append(value);
		return _stringBuilder.ToString();
	}
	private string BuildRangeString(uint min, uint max)
	{
		_stringBuilder.Clear();
		_stringBuilder.Append(min);
		_stringBuilder.Append(" / ");
		_stringBuilder.Append(max);
		return _stringBuilder.ToString();
	}
}
