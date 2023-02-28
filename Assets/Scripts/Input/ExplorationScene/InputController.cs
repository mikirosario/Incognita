using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputController
{
	public InputCommon InputCommon { get; }
}
public class InputController : MonoBehaviour, IInputController
{
	[SerializeField] private InputExploration _inputExploration;
	[SerializeField] private InputCommon _inputCommon;
	public InputExploration InputExploration { get { return _inputExploration; } set { _inputExploration = value; } }
	public InputCommon InputCommon { get { return _inputCommon; } set { _inputCommon = value; } }
}
