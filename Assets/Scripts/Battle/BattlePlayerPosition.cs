using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerPosition : MonoBehaviour, BattleSpawnPointSelector.IPosition
{
	public Transform Transform { get { return gameObject.transform; } }
	public string Name { get { return gameObject.name; } }

	public bool Equals(BattleSpawnPointSelector.IPosition other)
	{
		return this.Name == other.Name;
	}
}
