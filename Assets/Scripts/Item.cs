using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
	private uint _weight;
	private uint _volume;
	private string _label;
	private string _description;
	private Sprite _icon;
	public uint Weight { get; protected set; }
	public uint Volume { get; protected set; }
	public uint Label { get; protected set; }
	public string Description { get; protected set; }
	public Sprite Icon { get; protected set; }

	protected Item (uint weight, uint volume, uint label, string description, Sprite icon)
	{
		Weight = weight;
		Volume = volume;
		Label = label;
		Description = description;
		Icon = icon;
	}
}
