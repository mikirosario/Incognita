using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterScriptable : ScriptableObject
{
	[SerializeField] private string _name;
	[SerializeField] private Color _color;
	[SerializeField] uint _level;
	[Header("Hit Points")]
	[SerializeField] uint _hitPointsMax;
	[SerializeField] uint _hitPointsCurrent;
	[Header("Resonance Points")]
	[SerializeField] uint _resonancePointsMax;
	[SerializeField] uint _resonancePointsCurrent;
	[Header("Attributes")]
	[SerializeField] private CharacterAttribute _attack;
	[SerializeField] private CharacterAttribute _defence;
	[SerializeField] private CharacterAttribute _shield;
	[SerializeField] private CharacterAttribute _hitChance;
	[SerializeField] private CharacterAttribute _evasion;

	public void SetMaxValueInspector()
	{

	}

	public string Name { get { return _name; } internal set { _name = value; } }
	public Color Color { get { return _color; } internal set { _color = value; } }
	public uint Level { get { return _level; } internal set { _level = value; } }
	public uint HitPointsMax { get { return _hitPointsMax; } internal set { _hitPointsMax = value; } }
	public uint HitPointsCurrent { get { return _hitPointsCurrent; } internal set { _hitPointsCurrent = value; } }
	public uint ResonancePointsMax { get { return _resonancePointsMax; } internal set { _resonancePointsMax = value; } }
	public uint ResonancePointsCurrent { get { return _resonancePointsCurrent; } internal set { _resonancePointsCurrent = value; } }
	public CharacterAttribute Attack { get { return _attack; } internal set { _attack = value; } }
	public CharacterAttribute Defence { get { return _defence; } internal set { _defence = value; } }
	public CharacterAttribute Shield { get { return _shield; } internal set { _shield = value; } }
	public CharacterAttribute HitChance { get { return _hitChance; } internal set { _hitChance = value; } }
	public CharacterAttribute Evasion { get { return _evasion; } internal set { _evasion = value; } }

	private void OnDisable()
	{
		Debug.Log("OnDisableCalled");
		Attack.ClearModifiers();
		Defence.ClearModifiers();
		Shield.ClearModifiers();
		HitChance.ClearModifiers();
		Evasion.ClearModifiers();
	}
	private void OnEnable()
	{
		Debug.Log("OnEnableCalled");
		Attack = null;
		Defence = null;
		Shield = null;
		HitChance = null;
		Evasion = null;
		System.GC.Collect();
		Attack = new CharacterAttribute(null, null, 10);
		Defence = new CharacterAttribute();
		Shield = new CharacterAttribute();
		HitChance = new CharacterAttribute();
		Evasion = new CharacterAttribute();
	}
}
