using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterScriptable : ScriptableObject
{
	[SerializeField] private string _name;
	[SerializeField] private Color _color;
	[SerializeField] private uint _level;
	[Header("Hit Points")]
	[SerializeField, Range(0, 9999)] private uint _hitPointsMax;
	[SerializeField, Range(0, 9999)] private uint _hitPointsCurrent;
	[Header("Resonance Points")]
	[SerializeField, Range(0, 9999)] private uint _resonancePointsMax;
	[SerializeField, Range(0, 9999)] private uint _resonancePointsCurrent;
	[Header("Attributes")]
	[SerializeField] private CharacterAttribute _attack = new CharacterAttribute(null, null, 9999);
	//[ContextMenuItem("Set Attack Max Value", nameof(SetAttackMaxValue))] public uint _newAttackMaxValue = uint.MaxValue;
	[SerializeField, Space] private CharacterAttribute _defence = new CharacterAttribute(null, null, 9999);
	//[ContextMenuItem("Set Defence Max Value", nameof(SetDefenceMaxValue))] public uint _newDefenceMaxValue = uint.MaxValue;
	[SerializeField, Space] private CharacterAttribute _shield = new CharacterAttribute(null, null, 9999);
	//[ContextMenuItem("Set Shield Max Value", nameof(SetShieldMaxValue))] public uint _newShieldMaxValue = uint.MaxValue;
	[SerializeField, Space] private CharacterAttribute _hitChance = new CharacterAttribute(null, null, 1000);
	//[ContextMenuItem("Set Hit Chance Max Value", nameof(SetHitChanceMaxValue))] public uint _newHitChanceMaxValue = uint.MaxValue;
	[SerializeField, Space] private CharacterAttribute _evasion = new CharacterAttribute(null, null, 1000);
	//[ContextMenuItem("Set Evasion Max Value", nameof(SetEvasionMaxValue))] public uint _newEvasionMaxValue = uint.MaxValue;

	//public void SetAttackMaxValue()
	//{
	//	Attack.SetMaxValue(_newAttackMaxValue);
	//}
	//public void SetDefenceMaxValue()
	//{
	//	Defence.SetMaxValue(_newDefenceMaxValue);
	//}
	//public void SetShieldMaxValue()
	//{
	//	Shield.SetMaxValue(_newShieldMaxValue);
	//}
	//public void SetHitChanceMaxValue()
	//{
	//	HitChance.SetMaxValue(_newHitChanceMaxValue);
	//}
	//public void SetEvasionMaxValue()
	//{
	//	Evasion.SetMaxValue(_newEvasionMaxValue);
	//}
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
		//Debug.Log("OnDisableCalled");
		//Attack.ClearModifiers();
		//Defence.ClearModifiers();
		//Shield.ClearModifiers();
		//HitChance.ClearModifiers();
		//Evasion.ClearModifiers();
	}
	private void OnEnable()
	{
		//Debug.Log("OnEnableCalled");
		//Attack = null;
		//Defence = null;
		//Shield = null;
		//HitChance = null;
		//Evasion = null;
		//System.GC.Collect();
		//Attack = new CharacterAttribute(null, null, 9999);
		//Defence = new CharacterAttribute(null, null, 9999);
		//Shield = new CharacterAttribute(null, null, 9999);
		//HitChance = new CharacterAttribute(null, null, 1000);
		//Evasion = new CharacterAttribute(null, null, 1000);
	}
}
