using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tonic : IItem, IConsumable
{
	static private readonly uint _weight = 1;
	static private readonly uint _usesMax = 1;
	static private readonly string _label = "Tonic"; //retrieve from LocalizationManager -Miki
	static private readonly string _description = "A delicious cocktail packed with essential vitamins and nutrients."; //retrieve from Localization Manager -Miki
	static private readonly Sprite _icon; //either serialize this and set it in the UnityEditor or use LoadResource to set it directly from the item icons folder.
	static private readonly uint _healing = 10;
	private uint _uses = 1;

	public uint Weight { get; }
	public string Label { get; }
	public string Description { get; }
	public Sprite Icon { get; }
	public uint Healing { get; }
	public uint Uses { get; set; }
	public void ApplyEffect(Character character)
	{
		if (Uses > 0)
		{
			character.ReceiveHealing(Healing);
			Uses -= 1;
		}
	}
}
