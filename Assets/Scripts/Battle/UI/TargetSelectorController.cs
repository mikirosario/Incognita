using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectorController : MonoBehaviour
{
	[SerializeField] private Image _selectorIcon;

	private Image SelectorIcon { get { return _selectorIcon; } set { _selectorIcon = value; } }

	private void Awake()
	{
		
	}

	public void SelectTarget(IPhysical target)
	{
		SelectorIcon.transform.position = new Vector3(target.Transform.position.x, target.Transform.position.y + 1f, target.Transform.position.z);
		SelectorIcon.gameObject.SetActive(true);
	}

	public void DeselectTarget()
	{
		SelectorIcon.transform.localPosition = Vector3.zero;
		SelectorIcon.gameObject.SetActive(false);
	}
}
