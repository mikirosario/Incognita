using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
	[SerializeField] private Character _kai;

	public Character Kai { get { return _kai; } }

	/* //Some testing
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
			Kai.ReceiveAttackModifier(new CharacterAttribute.Modifier("test", uint.MaxValue), CharacterAttribute.ModifierType.Bonus);
		if (Input.GetKeyDown(KeyCode.E))
			Kai.ReceiveAttackModifier(new CharacterAttribute.Modifier("test", uint.MaxValue), CharacterAttribute.ModifierType.Malus);
	}
	*/
}
