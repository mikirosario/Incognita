using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : ScriptableObject
{
	public bool isPartyMemberKai;
	public bool isPartyMemberNami;
	public bool isPartyMemberMarlin;

	SaveData()
	{
		isPartyMemberKai = false;
		isPartyMemberNami = false;
		isPartyMemberMarlin = false;
	}
	SaveData(SaveData other)
	{
		isPartyMemberKai = other.isPartyMemberKai;
		isPartyMemberNami = other.isPartyMemberNami;
		isPartyMemberMarlin = other.isPartyMemberMarlin;
	}
}
