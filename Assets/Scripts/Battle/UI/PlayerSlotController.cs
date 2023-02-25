using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSlotController : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _textObject;
	//[SerializeField] private HealthBarController _healthBar;

	public TextMeshProUGUI TextObject { get { return _textObject; } }
}
