using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaSelector : MonoBehaviour
{
	public Dictionary<string, IBattleAreaController> BattleArea { get; private set; }

	private void Awake()
	{
		IBattleAreaController[] areas = GetComponentsInChildren<IBattleAreaController>(true);
		BattleArea = new Dictionary<string, IBattleAreaController>(areas.Length);
		foreach (IBattleAreaController area in areas)
			BattleArea.Add(area.Name, area);
	}
	public IBattleAreaController Get(string name)
	{
		return BattleArea[name];
	}
}
