using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCommon : MonoBehaviour
{
	[SerializeField] private Common_PauseToggle _common_PauseToggle;

	public Common_PauseToggle PauseToggle { get { return _common_PauseToggle; } }
}
