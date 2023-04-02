using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAreaSelector : MonoBehaviour
{
	public Dictionary<string, IBattleAreaController> BattleArea { get; private set; }
	public Dictionary<string, GameObject> BattleAreaObjects { get; private set; }

	private void Awake()
	{
		BattleAreaObjects = new Dictionary<string, GameObject>(transform.childCount);
		for (int i = 0; i < transform.childCount; ++i)
		{
			Transform child = transform.GetChild(i);
			BattleAreaObjects.Add(child.gameObject.name, child.gameObject);
			child = null;
		}
		IBattleAreaController[] areas = GetComponentsInChildren<IBattleAreaController>(true);
		BattleArea = new Dictionary<string, IBattleAreaController>(areas.Length);
		foreach (IBattleAreaController area in areas)
			BattleArea.Add(area.Name, area);
	}
	public IBattleAreaController GetBattleAreaController(string name)
	{
		IBattleAreaController ret;
		bool areaExists;
		areaExists = BattleArea.TryGetValue(name, out ret);
		if (!areaExists)
			GameManager.Instance.BattleManager.Error = BattleManager.IAmError.AreaControllerNotFound;
		else
			GameManager.Instance.BattleManager.Error = BattleManager.IAmError.None;
		return areaExists ? ret : null;
	}

	public GameObject GetBattleAreaGameObject(string name)
	{
		GameObject ret;
		bool areaExists;
		areaExists = BattleAreaObjects.TryGetValue(name, out ret);
		if (!areaExists)
			GameManager.Instance.BattleManager.Error = BattleManager.IAmError.AreaNotFound;
		else
			GameManager.Instance.BattleManager.Error = BattleManager.IAmError.None;
		return areaExists ? ret : null;
	}
}
