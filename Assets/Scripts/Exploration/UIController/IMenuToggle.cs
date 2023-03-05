using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuToggle
{
	public IMenuToggle Next { get; set; }
	public IMenuToggle Prev { get; set; }
	public GameObject GameObject { get; }
	public bool IsDisplayed { get; }
	public void Display(bool doDisplay);
}
