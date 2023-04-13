using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Exploration_CharacterSheetToggle : MonoBehaviour, IMenuToggle
{
	//[SerializeField] private PlayerInput _playerInput;
	[SerializeField] private GameObject _characterSheetPanel;
	[SerializeField] private TextMeshProUGUI	_displayedAttackValue, _displayedDefenceValue,
												_displayedShieldValue, _displayedHitChanceValue,
												_displayedEvasionValue, _displayedHitPoints,
												_displayedResonancePoints;
	//[SerializeField] private PlayerInput _playerInput;
	private PlayerInput PlayerInput => GameManager.Instance.PlayerInput;
	private StringBuilder _stringBuilder = new StringBuilder(20);
	//private PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }
	private PlayerController PlayerController => GameManager.Instance.ExplorationManager.PlayerController;
	private TextMeshProUGUI DisplayedAttackValue { get { return _displayedAttackValue; } }
	private TextMeshProUGUI DisplayedDefenceValue { get { return _displayedDefenceValue; } }
	private TextMeshProUGUI DisplayedShieldValue { get { return _displayedShieldValue; } }
	private TextMeshProUGUI DisplayedHitChanceValue { get { return _displayedHitChanceValue; } }
	private TextMeshProUGUI DisplayedEvasionValue { get { return _displayedEvasionValue; } }
	private TextMeshProUGUI DisplayedHitPoints { get { return _displayedHitPoints; } }
	private TextMeshProUGUI DisplayedResonancePoints { get { return _displayedResonancePoints; } }
	public GameObject CharacterSheetPanel { get { return _characterSheetPanel; } }
	public GameObject GameObject { get { return CharacterSheetPanel; } }
	public IMenuToggle Next { get; set; }
	public IMenuToggle Prev { get; set; }
	public bool IsDisplayed { get; private set; }
	private void Start()
	{
		PlayerInput.actions.FindActionMap("Exploration").FindAction("CharacterSheetToggle").performed += OnCharacterSheetToggle;
	}

	public void Display(bool doDisplay)
	{
		//if (doDisplay == true && GameManager.Instance.ExplorationManager.Paused == true) //Can't display menu while paused
		//	return;
		IsDisplayed = doDisplay;
		//GameManager.Instance.ExplorationManager.InputController.Common.PauseToggle.PauseGame(doDisplay);
		if (doDisplay == true)
			RefreshStats();
		CharacterSheetPanel.SetActive(doDisplay);
	}

	private void RefreshStats()
	{
		DisplayedAttackValue.text = BuildValueString(PlayerController.Kai.Attack.Attribute);
		DisplayedDefenceValue.text = BuildValueString(PlayerController.Kai.Defence.Attribute);
		DisplayedShieldValue.text = BuildValueString(PlayerController.Kai.Shield.Attribute);
		DisplayedHitChanceValue.text = BuildValueString(PlayerController.Kai.HitChance.Attribute);
		DisplayedEvasionValue.text = BuildValueString(PlayerController.Kai.Evasion.Attribute);
		DisplayedHitPoints.text = BuildRangeString(PlayerController.Kai.HitPointsCurrent, PlayerController.Kai.HitPointsMax);
		DisplayedResonancePoints.text = BuildRangeString(PlayerController.Kai.ResonancePointsCurrent, PlayerController.Kai.ResonancePointsMax);
	}

	private void OnCharacterSheetToggle(InputAction.CallbackContext context)
	{
		//Display(!CharacterSheetPanel.activeSelf);
		GameManager.Instance.ExplorationManager.UIController.ToggleMenu(this);
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
