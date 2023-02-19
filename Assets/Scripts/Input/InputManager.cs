using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField] private Exploration_PlayerMove _exploration_PlayerMove;
	[SerializeField] private Common_Pause _common_Pause;
	public Exploration_PlayerMove Exploration_PlayerMove { get { return _exploration_PlayerMove; } }
	public Common_Pause Common_Pause { get { return _common_Pause; } }

}
