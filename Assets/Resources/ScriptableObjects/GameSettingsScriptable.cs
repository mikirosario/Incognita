using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettingsScriptable : ScriptableObject
{
	[SerializeField] private LocaleSelector.LocaleID _locale = LocaleSelector.LocaleID.EN;
	public LocaleSelector.LocaleID Locale { get { return _locale; } set { _locale = value; } }
}
