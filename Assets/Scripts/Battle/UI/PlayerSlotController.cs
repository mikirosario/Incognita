using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSlotController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _textObject;
	[SerializeField] private HealthBarController _healthBar;
	private Character _character;

	public TextMeshProUGUI TextObject { get { return _textObject; } }
	private HealthBarController HealthBarController { get { return _healthBar; } }
	public Character Character { get { return _character; } set { _character = value; TextObject.text = Character.Name; TextObject.color = Color.white; Character.DamageReceivedActions += UpdatePlayerSlot; UpdatePlayerSlot(); } }

	public void UpdatePlayerSlot()
	{
		HealthBarController.UpdateHealthBar(Character);
	}
}
