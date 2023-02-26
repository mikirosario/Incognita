using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour, ICharacter
{
	//[SerializeField] private string _name;
	[SerializeField] private RuntimeAnimatorController _explorationModeAnimatorController;
	[SerializeField] private RuntimeAnimatorController _battleModeAnimatorController;
	[SerializeField] private CharacterScriptable _characterData;
	public string Name { get { return CharacterData.Name; } }
	public Color Color { get { return CharacterData.Color; } private set { CharacterData.Color = value; } }
	public uint HitPointsMax { get { return CharacterData.HitPointsMax; } private set { CharacterData.HitPointsMax = value; } }
	public uint HitPointsCurrent { get { return CharacterData.HitPointsCurrent; } private set { CharacterData.HitPointsCurrent = value; } }
	public uint ResonancePointsMax { get { return CharacterData.ResonancePointsMax; } private set { CharacterData.ResonancePointsMax = value; } }
	public uint ResonancePointsCurrent { get { return CharacterData.ResonancePointsCurrent; } private set { CharacterData.ResonancePointsCurrent = value; } }
	public CharacterAttribute Attack { get { return CharacterData.Attack; } private set { CharacterData.Attack = value; } }
	public CharacterAttribute Defence { get { return CharacterData.Defence; } private set { CharacterData.Defence = value; } }
	public CharacterAttribute Shield { get { return CharacterData.Shield; } private set { CharacterData.Shield = value; } }
	public CharacterAttribute HitChance { get { return CharacterData.HitChance; } private set { CharacterData.HitChance = value; } }
	public CharacterAttribute Evasion { get { return CharacterData.Evasion; } private set { CharacterData.Evasion = value; } }
	public uint Level { get { return CharacterData.Level; } private set { CharacterData.Level = value; } }
	private CharacterScriptable CharacterData { get { return _characterData; } }
	public Animator Animator { get; private set; }
	private RuntimeAnimatorController AnimatorControllerExploration { get { return _explorationModeAnimatorController; } }
	private RuntimeAnimatorController AnimatorControllerBattle { get { return _battleModeAnimatorController; } }

	private void Awake()
	{
		Animator = GetComponent<Animator>();
		//The time has come. ;p Init(12, 6, 12, 12, 3, 8, 4); //these will probably eventually be supplied by a scriptable object coming from a save file
	}

	public void ReceiveDamage(uint damage)
	{
		if (HitPointsCurrent > 0)
			HitPointsCurrent = damage > HitPointsCurrent ? 0 : HitPointsCurrent - damage;
	}

	public void ReceiveHealing(uint healing)
	{
		uint hitPointsToMax = HitPointsMax - HitPointsCurrent;

		if (hitPointsToMax > 0)
			HitPointsCurrent = healing > hitPointsToMax ? HitPointsMax : HitPointsCurrent + healing;
	}

	public void CauseDamage(uint damage, Character target)
	{
		target.ReceiveDamage(damage);
	}

	public void CauseHealing(uint healing, Character target)
	{
		target.ReceiveHealing(healing);
	}

	public void ReceiveAttackModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type)
	{
		Attack.AddModifier(modifier, type);
	}

	public void ReceiveDefenceModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type)
	{
		Defence.AddModifier(modifier, type);
	}

	public void ReceiveShieldModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type)
	{
		Shield.AddModifier(modifier, type);
	}
	public void ReceiveHitChanceModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type)
	{
		HitChance.AddModifier(modifier, type);
	}
	public void ReceiveEvasionModifier(CharacterAttribute.Modifier modifier, CharacterAttribute.ModifierType type)
	{
		Evasion.AddModifier(modifier, type);
	}
	public void UseItem(IConsumable item, Character target)
	{
		item.ApplyEffect(target);
	}
	public void SetExplorationMode()
	{
		Animator.runtimeAnimatorController = AnimatorControllerExploration;
	}
	public void SetBattleMode()
	{
		Animator.runtimeAnimatorController = AnimatorControllerBattle;
	}

	private void Init(uint maxHP, uint maxRP, uint attack, uint defence, uint shield, uint hitChance, uint evasion)
	{
		Animator = GetComponent<Animator>();
		//HitPointsMax = maxHP;
		//HitPointsCurrent = HitPointsMax;
		//ResonancePointsMax = maxRP;
		//ResonancePointsCurrent = ResonancePointsMax;
		//Attack = new CharacterAttribute(attack);
		//Defence = new CharacterAttribute(defence);
		//Shield = new CharacterAttribute(shield);
		//HitChance = new CharacterAttribute(hitChance);
		//Evasion = new CharacterAttribute(evasion);
	}
	//public Character(uint maxHP, uint maxRP, uint attack, uint defence, uint shield, uint hitChance, uint evasion)
	//{
	//	HitPointsMax = maxHP;
	//	HitPointsCurrent = HitPointsMax;
	//	ResonancePointsMax = maxRP;
	//	ResonancePointsCurrent = ResonancePointsMax;
	//	Attack = new CharacterAttribute(attack);
	//	Defence = new CharacterAttribute(defence);
	//	Shield = new CharacterAttribute(shield);
	//	HitChance = new CharacterAttribute(hitChance);
	//	Evasion = new CharacterAttribute(evasion);
	//}
}
public interface ICharacter
{
	public string Name { get; }
	public Color Color { get; }
	public CharacterAttribute Attack { get; }
	public CharacterAttribute Defence { get; }
	public CharacterAttribute Shield { get; }
	public CharacterAttribute HitChance { get; }
	public CharacterAttribute Evasion { get; }
	public uint Level { get; }
	public uint ResonancePointsMax { get; }
	public uint ResonancePointsCurrent { get; }
	public uint HitPointsMax { get; }
	public uint HitPointsCurrent { get; }
	public void ReceiveDamage(uint damage);
	public void CauseDamage(uint damage, Character target);
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
[System.Serializable] public class CharacterAttribute
{
	public enum ModifierType
	{
		Bonus,
		Malus
	}
	[SerializeField] private uint _baseAttribute;
	public uint Attribute { get { return BaseAttribute + BonusTotal - MalusTotal; } }
	public uint BaseAttribute { get { return _baseAttribute; } private set { _baseAttribute = value; } }
	public BonusList Bonus { get; private set; }
	public MalusList Malus { get; private set; }
	public uint MalusTotal { get { return Modifier_SumValues(Malus); } }
	public uint BonusTotal { get { return Modifier_SumValues(Bonus); } }
	private StringBuilder ReusableString { get; }
	internal CharacterAttribute(BonusList bonusList = null, MalusList malusList = null)
	{
		BaseAttribute = 0;
		Bonus = bonusList == null ? new BonusList() : bonusList;
		Malus = malusList == null ? new MalusList() : malusList;
		ReusableString = new StringBuilder(20);
	}
	internal CharacterAttribute(uint baseAttribute, BonusList bonusList = null, MalusList malusList = null)
	{
		BaseAttribute = baseAttribute;
		Bonus = bonusList == null ? new BonusList() : bonusList;
		Malus = malusList == null ? new MalusList() : malusList;
		ReusableString = new StringBuilder(20);
	}

	internal CharacterAttribute(CharacterAttribute other)
	{
		BaseAttribute = other.BaseAttribute;
		Bonus = other.Bonus;
		Malus = other.Malus;
		ReusableString = other.ReusableString;
	}

	~CharacterAttribute()
	{
		Bonus = null;
		Malus = null;
		Debug.Log("Character Attribute Destructor Called");
	}

	public override string ToString()
	{
		ReusableString.Clear();
		ReusableString.Append(Attribute);
		return ReusableString.ToString();
	}
	private uint Modifier_SumValues(List<Modifier> modifierList)
	{
		uint sum = 0;
		foreach (Modifier modifier in modifierList)
			sum += modifier.Value;
		return sum;
	}
	private uint Modifier_SumBaseValueDifferences(List<Modifier> modifierList)
	{
		uint sum = 0;
		foreach (Modifier modifier in modifierList)
			sum += modifier.BaseValue - modifier.Value;
		return sum;
	}
	internal void AddModifier(Modifier newModifier, ModifierType attributeType)
	{
		if (attributeType == ModifierType.Malus)
		{
			if (newModifier.Value > Attribute)
				newModifier.Value = Attribute;
			Malus.Add(newModifier);
			if (newModifier.Value > 0)
				RecalculateBonuses();
		}
		else
		{
			if (newModifier.Value > uint.MaxValue - Attribute)
				newModifier.Value = uint.MaxValue - Attribute;
			Bonus.Add(newModifier);
			if (newModifier.Value > 0)
				RecalculateMaluses();
		}
	}

	internal bool RemoveModifier(string name, ModifierType attributeType)
	{
		Predicate<Modifier> pred = (Modifier modifier) => (modifier.Name.Equals(name));
		Modifier modifierToRemove;
		List<Modifier> modifierList = attributeType == ModifierType.Malus ? Malus : Bonus;

		if ((modifierToRemove = modifierList.Find(pred)) != null)
		{
			modifierList.Remove(modifierToRemove);
			RecalculateAttribute();
			return true;
		}
		return false;
	}

	private void RecalculateBonuses()
	{
		uint pointsToMax = uint.MaxValue - Attribute;
		for (int i = 0; pointsToMax > 0 && i < Bonus.Count; ++i)
		{
			Modifier modifier = Bonus[i];
			if (modifier.BaseValue > modifier.Value)
			{
				uint baseValDiff = modifier.BaseValue - modifier.Value;
				uint addend = baseValDiff > pointsToMax ? pointsToMax : baseValDiff;
				modifier.Value += addend;
				pointsToMax -= addend;
			}
		}
	}
	private void RecalculateMaluses()
	{
		uint attribute = Attribute;
		for (int i = 0; attribute > 0 && i < Malus.Count; ++i)
		{
			Modifier modifier = Malus[i];
			if (modifier.BaseValue > modifier.Value)
			{
				uint baseValDiff = modifier.BaseValue - modifier.Value;
				uint subtrahend = baseValDiff > attribute ? attribute : baseValDiff;
				modifier.Value += subtrahend;
				attribute -= subtrahend;
			}
		}
	}

	private void RecalculateAttribute()
	{
		while (Modifier_SumBaseValueDifferences(Bonus) > 0 && Modifier_SumBaseValueDifferences(Malus) > 0)
		{
			RecalculateBonuses();
			RecalculateMaluses();
		}
	}
	[System.Serializable] public class Modifier : IEquatable<Modifier>
	{
		[SerializeField] private string _name;
		private uint _value;
		[SerializeField] private uint _baseValue;

		public string Description
		{
			get { return string.Format("{0} from {1}", Value, Name); } //Use with Localization.Manager and BuildString -Miki
		}
		public string Name { get; private set; }
		public uint Value
		{
			get { return _value; }
			internal set { _value = value > BaseValue ? BaseValue : value; }
		}
		public uint BaseValue { get { return _baseValue; } private set { _baseValue = value; } }

		public Modifier(string name, uint value)
		{
			Name = name;
			BaseValue = value;
			Value = BaseValue;
		}
		public bool Equals(Modifier other)
		{
			if (other != null && this.Name.Equals(other.Name))
				return true;
			return false;
		}
	}
	[System.Serializable]
	public class MalusList : List<Modifier> {}
	public class BonusList : List<Modifier> {}
}
