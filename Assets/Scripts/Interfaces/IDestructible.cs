using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible: IPhysical
{
	public uint HitPointsMax { get; }
	public uint HitPointsCurrent { get; }
	public void ReceiveDamage(uint damage);
	public void DisplayDamage(uint damage, DamageNumber.Effect effect = DamageNumber.Effect.SimpleRise);
	//public void Destruct();
}
