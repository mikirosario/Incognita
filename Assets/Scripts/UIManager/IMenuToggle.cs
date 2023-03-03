using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuToggle
{
	public GameObject GameObject { get; }
	public bool IsDisplayed { get; }
	public void Display(bool doDisplay);
}
