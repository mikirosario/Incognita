using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRowBounds : MonoBehaviour
{
	[SerializeField] private RectTransform _rectTransform;
	private float LeftBound { get { return _rectTransform.position.x - _rectTransform.sizeDelta.x * 0.5f; } }
	private float RightBound { get { return _rectTransform.position.x + _rectTransform.sizeDelta.x * 0.5f; } }
	public bool Contains(Transform transform)
	{
		bool ret = true;
		if (transform.position.x < LeftBound || transform.position.x > RightBound)
			ret = false;
		return ret;
	}
}
