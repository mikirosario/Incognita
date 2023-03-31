using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter : IDestructible
{
	//public Transform Transform { get; } Now comes from IDestructible
	public string Name { get; }
	public Color Color { get; }
	public uint Level { get; }
	//public uint HitPointsMax { get; } Now comes from IDestructible
	//public uint HitPointsCurrent { get; } Now comes from IDestructible
	public uint ResonancePointsMax { get; }
	public uint ResonancePointsCurrent { get; }
	public CharacterAttribute Attack { get; }
	public CharacterAttribute Defence { get; }
	public CharacterAttribute Shield { get; }
	public CharacterAttribute HitChance { get; }
	public CharacterAttribute Evasion { get; }
	public IEnumerator AttackTarget(IDestructible target);
	//public void ReceiveDamage(uint damage); //Now comes from IDestructible
	public void CauseDamage(uint damage, IDestructible target);
	public void ReceiveHealing(uint healing);
	public void CauseHealing(uint healing, Character target);
	public void ReceiveAttackModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type);
	public void ReceiveDefenceModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type);
	public void ReceiveShieldModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type);
	public void ReceiveHitChanceModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type);
	public void ReceiveEvasionModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type);
	public void UseItem(IConsumable item, Character target);
	public void SetExplorationMode();
	public void SetBattleMode();
}
