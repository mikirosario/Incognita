using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleAreaController
{
	//public enum Area
	//{
	//	Town = 0,
	//	//City,
	//	//Vents,
	//	//Reefs,
	//	//KelpForest,
	//	//Abyss,
	//	//Trenches,
	//	//Caverns,
	//	//Crags,
	//	//Peaks,
	//	//Roof,
	//	//Surface,
	//	//Anomaly,
	//	//VirtualWorld,
	//	//AnomalyCore
	//}
	public string Name { get; }
	//public Area AreaKey { get; }
	public List<GameObject> Enemies { get; }
	public BattleSpawnPointSelector SpawnPointSelector { get; }
}
