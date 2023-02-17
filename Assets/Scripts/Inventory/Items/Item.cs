using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItem
{
	public uint Weight { get; }
	public string Label { get; }
	public string Description { get; }
	public Sprite Icon { get; }
}

public interface IConsumable
{
	public uint Uses { get; set; }
	public void ApplyEffect(Character character);
}

public interface IEquippable
{
	string EffectAndCause { get; } //eg, "+1 from {itemLabel}"
	public void ApplyEffect(Character character);
	public void RemoveEffect(Character character);
}

//public abstract class Equippable : Item
//{

//}

//private uint _quantity;
// private uint _weight;
// private uint _volume; move all this to object level
//private string _label;
//private string _description;
// private Sprite _icon;

//public uint Volume
//{
//	get { return Volume * Quantity; }
//	protected set { Volume = value; }
//}
//static public uint Label { get; protected set; }
//static public string Description { get; protected set; }
//static public Sprite Icon { get; protected set; }

//protected Item(uint quantity = 0)
//{
//	Quantity = quantity;
//	Weight = weight;
//	Volume = volume;
//	Label = label;
//	Description = description;
//	Icon = icon;
//}