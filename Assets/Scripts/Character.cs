using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	private uint level;

	// Base stats, with no equipment
	private uint baseAttack;
	private uint baseDefence;
	private uint baseShield;                //Magic Defence
	private uint baseHitChance;
	private uint baseEvasion;

	//Max should be determined by base defence + equipment modifiers
	private uint hitPointsMax;
	private uint resonancePointsMax;        //Magic Points
	
	private uint hitPointsCurrent;
	private uint resonancePointsCurrent;



}
