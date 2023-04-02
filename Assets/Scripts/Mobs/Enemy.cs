using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IMob
{
	private uint _hitPointsCurrent;
	private uint _resonancePointsCurrent;
	private CharacterAttribute _attack;
	private CharacterAttribute _defence;
	private CharacterAttribute _shield;
	private CharacterAttribute _hitChance;
	private CharacterAttribute _evasion;
	override public uint HitPointsCurrent { get { return _hitPointsCurrent; } protected set { _hitPointsCurrent = value; } }
	override public uint ResonancePointsCurrent { get { return _resonancePointsCurrent; } protected set { _resonancePointsCurrent = value; } }
	override public CharacterAttribute Attack { get { return _attack; } protected set { _attack = value; } }
	override public CharacterAttribute Defence { get { return _defence; } protected set { _defence = value; } }
	override public CharacterAttribute Shield { get { return _shield; } protected set { _shield = value; } }
	override public CharacterAttribute HitChance { get { return _hitChance; } protected set { _hitChance = value; } }
	override public CharacterAttribute Evasion { get { return _evasion; } protected set { _evasion = value; } }
	private void Awake()
	{
		HitPointsCurrent = base.HitPointsMax;
		ResonancePointsCurrent = base.ResonancePointsMax;
		Attack = base.Attack;
		Defence = base.Defence;
		Shield = base.Shield;
		HitChance = base.HitChance;
		Evasion = base.Evasion;
	}
}
