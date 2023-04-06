using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible: IPhysical
{
	public Action DamageReceived { get; set; }
	public uint HitPointsMax { get; }
	public uint HitPointsCurrent { get; }
	public void ReceiveDamage(uint damage);
	public void DisplayDamage(uint amount, DamageNumber.Type type = DamageNumber.Type.Damage, DamageNumber.Effect effect = DamageNumber.Effect.SimpleRise);
	//public void Destruct();
}
