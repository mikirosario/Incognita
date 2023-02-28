using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
	[SerializeField] private InputExploration _inputExploration;
	[SerializeField] private InputCommon _inputCommon;
	[SerializeField] private PlayerInput _playerInput;
	public InputExploration InputExploration { get { return _inputExploration; } set { _inputExploration = value; } }
	public InputCommon InputCommon { get { return _inputCommon; } set { _inputCommon = value; } }
	public PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }

	private void Awake()
	{
		PlayerInput.actions.FindActionMap("Exploration").Enable();
		PlayerInput.actions.FindActionMap("Common").Enable();
	}
}
