using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class CharacterSheetController : MonoBehaviour
{
	[SerializeField] private GameObject _characterSheetPanel;
	[SerializeField] private TextMeshProUGUI _displayedAttackValue, _displayedDefenceValue, _displayedShieldValue, _displayedHitChanceValue, _displayedEvasionValue, _displayedHitPoints, _displayedResonancePoints;
	private StringBuilder _stringBuilder = new StringBuilder(20);
	private char[] _str = new char[20];
	private PlayerManager PlayerManager => GameManager.Instance.PlayerManager;
	private TextMeshProUGUI DisplayedAttackValue { get { return _displayedAttackValue; } }
	private TextMeshProUGUI DisplayedDefenceValue { get { return _displayedDefenceValue; } }
	private TextMeshProUGUI DisplayedShieldValue { get { return _displayedShieldValue; } }
	private TextMeshProUGUI DisplayedHitChanceValue { get { return _displayedHitChanceValue; } }
	private TextMeshProUGUI DisplayedEvasionValue { get { return _displayedEvasionValue; } }
	private TextMeshProUGUI DisplayedHitPoints { get { return _displayedHitPoints; } }
	private TextMeshProUGUI DisplayedResonancePoints { get { return _displayedResonancePoints; } }
	public GameObject CharacterSheetPanel { get { return _characterSheetPanel; } }
	private void OnCharacterSheetToggle()
	{
		DisplayedAttackValue.text = BuildValueString(PlayerManager.Kai.Attack.Attribute);
		DisplayedDefenceValue.text = BuildValueString(PlayerManager.Kai.Defence.Attribute);
		DisplayedShieldValue.text = BuildValueString(PlayerManager.Kai.Shield.Attribute);
		DisplayedHitChanceValue.text = BuildValueString(PlayerManager.Kai.HitChance.Attribute);
		DisplayedEvasionValue.text = BuildValueString(PlayerManager.Kai.Evasion.Attribute);
		DisplayedHitPoints.text = BuildRangeString(PlayerManager.Kai.HitPointsCurrent, PlayerManager.Kai.HitPointsMax);
		DisplayedResonancePoints.text = BuildRangeString(PlayerManager.Kai.ResonancePointsCurrent, PlayerManager.Kai.ResonancePointsMax);
		CharacterSheetPanel.SetActive(!CharacterSheetPanel.activeSelf);
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
