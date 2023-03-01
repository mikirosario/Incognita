using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
	bool isActiveCoroutine = false;
	enum Language
	{
		EN = 0,
		ES = 1 
	}
	private void ChangeLanguage(Language languageID)
	{
		if (isActiveCoroutine)
			return;
		StartCoroutine(SetLocaleAsync((int)languageID));
	}
	IEnumerator SetLocaleAsync(int localeID)
	{
		isActiveCoroutine = true;
		yield return LocalizationSettings.InitializationOperation;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
		isActiveCoroutine = false;
	}
}
