using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour, ICharacter, IDestructible, IPhysical
{
	protected enum CharacterMode
	{
		EXPLORATION = 0,
		BATTLE,
		NONE
	}
	[SerializeField] private CharacterMode _mode = CharacterMode.NONE;
	[SerializeField] private Animator _animator;
	[SerializeField] private RuntimeAnimatorController _explorationModeAnimatorController;
	[SerializeField] private RuntimeAnimatorController _battleModeAnimatorController;
	[SerializeField] private CharacterScriptable _characterData;
	[SerializeField] private GameObject _damageNumPrefab;
	private Action _damageReceivedActions = null;
	public Transform Transform { get { return this.transform; } }
	public string Name { get { return CharacterData.Name; } }
	public Color Color { get { return CharacterData.Color; } protected set { CharacterData.Color = value; } }
	public uint HitPointsMax { get { return CharacterData.HitPointsMax; } protected set { CharacterData.HitPointsMax = value; } }
	virtual public uint HitPointsCurrent { get { return CharacterData.HitPointsCurrent; } protected set { CharacterData.HitPointsCurrent = value; } }
	public uint ResonancePointsMax { get { return CharacterData.ResonancePointsMax; } protected set { CharacterData.ResonancePointsMax = value; } }
	virtual public uint ResonancePointsCurrent { get { return CharacterData.ResonancePointsCurrent; } protected set { CharacterData.ResonancePointsCurrent = value; } }
	virtual public CharacterAttribute Attack { get { return CharacterData.Attack; } protected set { CharacterData.Attack = value; } }
	virtual public CharacterAttribute Defence { get { return CharacterData.Defence; } protected set { CharacterData.Defence = value; } }
	virtual public CharacterAttribute Shield { get { return CharacterData.Shield; } protected set { CharacterData.Shield = value; } }
	virtual public CharacterAttribute HitChance { get { return CharacterData.HitChance; } protected set { CharacterData.HitChance = value; } }
	virtual public CharacterAttribute Evasion { get { return CharacterData.Evasion; } protected set { CharacterData.Evasion = value; } }
	public uint Level { get { return CharacterData.Level; } protected set { CharacterData.Level = value; } }
	public Animator Animator { get { return _animator; } protected set { _animator = value; } }
	public Action DamageReceivedActions { get { return _damageReceivedActions; } set { _damageReceivedActions = value; } }
	protected CharacterScriptable CharacterData { get { return _characterData; } }
	protected RuntimeAnimatorController AnimatorControllerExploration { get { return _explorationModeAnimatorController; } }
	protected RuntimeAnimatorController AnimatorControllerBattle { get { return _battleModeAnimatorController; } }
	protected CharacterMode Mode { get { return _mode; } set { _mode = value; } }
	protected GameObject DamageNumPrefab { get { return _damageNumPrefab; } }

	private void Awake()
	{
		if (GameManager.Instance.ActiveScene.Equals(GameManager.SceneIndex.ExplorationScene))
			SetExplorationMode();
		else if (GameManager.Instance.ActiveScene.Equals(GameManager.SceneIndex.BattleScene))
			SetBattleMode();
	}

	public IEnumerator AttackTarget(IDestructible target)
	{
		if (Mode.Equals(CharacterMode.BATTLE))
		{
			List<InputAction> inputActions = InputSystemExt.DisableInputs();
			Vector2 originalPos = this.Transform.position;
			Vector2 targetPos = new Vector2(target.Transform.position.x - 1f, target.Transform.position.y);
			Animator.Play("Run_Right_BattleKai");
			yield return new WaitUntil(() => (Vector2)(this.Transform.position = Vector2.MoveTowards(this.Transform.position, targetPos, 8 * Time.deltaTime)) == targetPos);
			Animator.Play("Attack_Right_BattleKai");
			yield return new WaitUntil(() => Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
			Animator.Play("Run_Right_BattleKai");
			//play attack animation
			CauseDamage(20, target); //need to decide whether to use scriptables for enemies or not
			//display damage
			target.DisplayDamage(20, DamageNumber.Type.Posion);
			yield return new WaitUntil(() => (Vector2)(this.Transform.position = Vector2.MoveTowards(this.Transform.position, originalPos, 8 * Time.deltaTime)) == originalPos);
			Animator.Play("Idle_Right_BattleKai");
			InputSystemExt.EnableInputs(inputActions);
			inputActions.Clear();
			//restore user inputs
		}
	}

	public void DisplayDamage(uint amount, DamageNumber.Type type = DamageNumber.Type.Damage, DamageNumber.Effect effect = DamageNumber.Effect.SimpleRise)
	{
		GameObject damageNumberObject = Instantiate(DamageNumPrefab, transform);
		DamageNumber damageNumberScript = damageNumberObject.GetComponent<DamageNumber>();
		damageNumberObject.transform.position += new Vector3(0f, 1f, 0f);
		damageNumberScript.Color = DamageNumber.GetColor(type);
		damageNumberScript.Display(effect, amount);
	}
	public void ReceiveDamage(uint damage)
	{
		if (HitPointsCurrent > 0)
		{
			HitPointsCurrent = damage > HitPointsCurrent ? 0 : HitPointsCurrent - damage;
			DamageReceivedActions?.Invoke();
		}
	}

	public void ReceiveHealing(uint healing)
	{
		uint hitPointsToMax = HitPointsMax - HitPointsCurrent;

		if (hitPointsToMax > 0)
			HitPointsCurrent = healing > hitPointsToMax ? HitPointsMax : HitPointsCurrent + healing;
	}

	public void CauseDamage(uint damage, IDestructible target)
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
		//Debug.Log("set exploration mode called");
		if (Mode == CharacterMode.EXPLORATION)
			return;
		if (Animator != null)
			Animator.runtimeAnimatorController = AnimatorControllerExploration;
		Mode = CharacterMode.EXPLORATION;
	}

	public void SetBattleMode()
	{
		//Debug.Log("set battle mode called");
		if (Mode == CharacterMode.BATTLE)
			return;
		if (Animator != null)
			Animator.runtimeAnimatorController = AnimatorControllerBattle;
		Mode = CharacterMode.BATTLE;
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
		//debug code
		//if (Input.GetKeyDown(KeyCode.Q))
		//	ReceiveAttackModifier(new CharacterAttribute.Modifier("bleh", 10), CharacterAttribute.ModifierType.Malus);
		//if (Input.GetKeyDown(KeyCode.E))
		//	ReceiveAttackModifier(new CharacterAttribute.Modifier("blu", 10), CharacterAttribute.ModifierType.Bonus);
		//if (Input.GetKeyDown(KeyCode.F))
		//	Attack.SetMaxValue(5);
		if (Input.GetKeyDown(KeyCode.H))
			ReceiveDamage(2);
	}
}

[System.Serializable] public class CharacterAttribute
{
	public enum ModifierType
	{
		Bonus,
		Malus
	}
	[SerializeField] private uint _baseAttribute;
	[ReadOnly, SerializeField] private uint _maxValue = uint.MaxValue;
	[SerializeField] private List<Modifier> _bonusList;
	[SerializeField] private List<Modifier> _malusList;
	public uint Attribute { get { return BaseAttribute + BonusTotal - MalusTotal; } }
	public uint BaseAttribute { get { return _baseAttribute; } private set { _baseAttribute = value; } }
	public BonusList Bonus { get { return (BonusList)_bonusList; } private set { _bonusList = value; } }
	public MalusList Malus { get { return (MalusList)_malusList; } private set { _malusList = value; } }
	public uint MalusTotal { get { return Modifier_SumValues(Malus); } }
	public uint BonusTotal { get { return Modifier_SumValues(Bonus); } }
	private StringBuilder ReusableString { get; }
	private uint MaxValue { get { return _maxValue; } set { _maxValue = value; } }
	private uint AttributePointsToMax { get { return MaxValue - Attribute; } }
	private uint BaseAttributePointsToMax { get { return MaxValue - BaseAttribute; } }
	internal CharacterAttribute(BonusList bonusList = null, MalusList malusList = null, uint maxValue = uint.MaxValue)
	{
		MaxValue = maxValue;
		BaseAttribute = 0;
		Bonus = bonusList == null ? new BonusList() : bonusList;
		Malus = malusList == null ? new MalusList() : malusList;
		ReusableString = new StringBuilder(20);
	}
	internal CharacterAttribute(uint baseAttribute, BonusList bonusList = null, MalusList malusList = null, uint maxValue = uint.MaxValue)
	{
		MaxValue = maxValue;
		BaseAttribute = baseAttribute > MaxValue ? MaxValue : baseAttribute;
		Bonus = bonusList == null ? new BonusList() : bonusList;
		Malus = malusList == null ? new MalusList() : malusList;
		ReusableString = new StringBuilder(20);
	}

	internal CharacterAttribute(CharacterAttribute other)
	{
		MaxValue = other.MaxValue;
		BaseAttribute = other.BaseAttribute;
		Bonus = other.Bonus;
		Malus = other.Malus;
		ReusableString = other.ReusableString;
	}

	~CharacterAttribute()
	{
		Bonus = null;
		Malus = null;
		//Debug.Log("Character Attribute Destructor Called");
	}

	public override string ToString()
	{
		ReusableString.Clear();
		ReusableString.Append(Attribute);
		return ReusableString.ToString();
	}
	public void ClearModifiers()
	{
		Bonus.Clear();
		Malus.Clear();
	}
	public void SetMaxValue(uint value)
	{
		if (value == MaxValue || value < BaseAttribute)
			return;
		if (value < Attribute)
		{
			foreach (Modifier bonus in Bonus)
				bonus.Value = 0;
			foreach (Modifier malus in Malus)
				malus.Value = 0;
			MaxValue = value;
			RecalculateAttribute();
		}
		else
			MaxValue = value;
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
		}
		else
		{
			if (newModifier.Value > AttributePointsToMax)
				newModifier.Value = AttributePointsToMax;
			Bonus.Add(newModifier);
		}
		if (newModifier.Value > 0)
			RecalculateAttribute();
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
	internal void RaiseBaseAttributeBy(uint value)
	{
		uint baseAttributePointsToMax = BaseAttributePointsToMax;
		uint attributePointsToMax = AttributePointsToMax;
		if (value > baseAttributePointsToMax)
			value = baseAttributePointsToMax;
		if (value > attributePointsToMax)
			value -= LowerModifiersBy(value - attributePointsToMax, Bonus);
		BaseAttribute += value;
	}
	internal void LowerBaseAttributeBy(uint value)
	{
		uint baseAttributePointsToMin = BaseAttribute;
		uint attributePointsToMin = Attribute;
		if (value > baseAttributePointsToMin)
			value = baseAttributePointsToMin;
		if (value > attributePointsToMin)
			value -= LowerModifiersBy(value - attributePointsToMin, Malus);
		BaseAttribute -= value;
	}
	private uint LowerModifiersBy(uint value, List<Modifier> modifierList)
	{
		if (modifierList.Count == 0 || value == 0)
			return value;
		int i = modifierList.Count - 1;
		do
		{
			if (modifierList[i].Value >= value)
			{
				modifierList[i].Value -= value;
				value = 0;
			}
			else
			{
				value -= modifierList[i].Value;
				modifierList[i].Value = 0;
			}
		} while (i-- > 0 && value > 0);
		return value;
	}
	private void RecalculateBonuses()
	{
		uint pointsToMax = AttributePointsToMax;
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
		uint leftoverBonusPoints;
		uint leftoverMalusPoints;
		leftoverBonusPoints = Modifier_SumBaseValueDifferences(Bonus);
		leftoverMalusPoints = Modifier_SumBaseValueDifferences(Malus);
		while ((leftoverBonusPoints > 0 && Attribute < MaxValue) || (leftoverMalusPoints > 0 && Attribute > 0))
		{
			RecalculateBonuses();
			RecalculateMaluses();
			leftoverBonusPoints = Modifier_SumBaseValueDifferences(Bonus);
			leftoverMalusPoints = Modifier_SumBaseValueDifferences(Malus);
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
		public string Name { get { return _name; } private set { _name = value; } }
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
	[System.Serializable] public class MalusList : List<Modifier> {}
	[System.Serializable] public class BonusList : List<Modifier> {}
}
