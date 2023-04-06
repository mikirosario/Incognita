using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarController : MonoBehaviour
{
	[SerializeField] private Image _healthBar;
	[SerializeField] private TextMeshProUGUI _healthVal;

	private Image Health { get { return _healthBar; } }
	private TextMeshProUGUI HealthVal { get {return _healthVal; } }
	
	public void UpdateHealthBar (Character character)
	{
		float fill = character.HitPointsCurrent / character.HitPointsMax;
		Health.fillAmount = fill;
		HealthVal.text = $"{character.HitPointsCurrent}/{character.HitPointsMax}";
	}	
}
