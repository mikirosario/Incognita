using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
	public enum LocaleID
	{
		EN = 0,
		ES = 1
	}
	[SerializeField] private GameSettingsScriptable _gameSettings;
	private bool IsRunning { get; set; }
	private LocaleID CurrentLocale { get { return _gameSettings.Locale; } set { _gameSettings.Locale = value; } }
	private void Awake()
	{
		ChangeLocale(CurrentLocale);
	}
	private IEnumerator SetLocale(LocaleID localeID)
	{
		IsRunning = true;
		yield return LocalizationSettings.InitializationOperation;
		LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)localeID];
		CurrentLocale = localeID;
		IsRunning = false;
	}
	public void ChangeLocale(LocaleID localeID)
	{
		if (IsRunning == false)
			StartCoroutine(SetLocale(localeID));
	}
	public void ChangeLocaleEnglish()
	{
		if (CurrentLocale != LocaleID.EN)
			ChangeLocale(LocaleID.EN);
	}
	public void ChangeLocaleSpanish()
	{
		if (CurrentLocale != LocaleID.ES)
			ChangeLocale(LocaleID.ES);
	}
}
