using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrossfadeController : MonoBehaviour, IScreenTransition
{
	private bool _isAnimating;
	[SerializeField] private CanvasGroup _crossfadeGroup;
	[SerializeField, Range(0.01f, 4f)] private float _animationDuration;
	public CanvasGroup CrossfadeGroup { get { return _crossfadeGroup; } }
	public IFadeOut FadeOut { get; private set; }
	public IFadeIn FadeIn { get; private set; }
	public bool IsAnimating { get { return _isAnimating; } internal set { _isAnimating = value; } }
	private float AnimationDuration { get { return _animationDuration; } set { _animationDuration = value; } }
	private void Awake()
	{
		FadeOut = new CrossfadeOut(CrossfadeGroup, AnimationDuration, ref _isAnimating);
		FadeIn = new CrossfadeIn(CrossfadeGroup, AnimationDuration, ref _isAnimating);
		IsAnimating = false;
	}
}
public abstract class Crossfade
{
	protected List<InputAction> ActionList { get; set; }
	protected CanvasGroup CrossfadeGroup { get; set; }
	protected float AnimationDuration { get; set; }
	public bool IsAnimating { get; protected set; }
	internal Crossfade(CanvasGroup canvasGroup, float animationDuration, ref bool isAnimating)
	{
		CrossfadeGroup = canvasGroup;
		AnimationDuration = animationDuration;
		IsAnimating = isAnimating;
	}
	~Crossfade()
	{
		ActionList.Clear();
		CrossfadeGroup = null;
	}
}

public class CrossfadeOut : Crossfade, IFadeOut
{
	public IEnumerator Play()
	{
		while (IsAnimating)
			yield return null;
		if (CrossfadeGroup.alpha < 1f)
		{
			IsAnimating = true;
			ActionList = InputSystem.ListEnabledActions();
			InputSystem.DisableAllEnabledActions();
			CrossfadeGroup.interactable = true;
			CrossfadeGroup.blocksRaycasts = true;
			CrossfadeGroup.alpha = 0f;
			Time.timeScale = 0;
			float timer = 0f;
			while (timer < AnimationDuration)
			{
				float step = 1f / ((AnimationDuration - timer) / Time.unscaledDeltaTime);
				CrossfadeGroup.alpha += step;
				yield return null;
				timer += Time.unscaledDeltaTime;
			}
			CrossfadeGroup.alpha = 1f;
			Time.timeScale = 1f;
			foreach (InputAction action in ActionList)
				action.Enable();
			ActionList.Clear();
			IsAnimating = false;
		}
	}
	internal CrossfadeOut(CanvasGroup canvasGroup, float animationDuration, ref bool isAnimating) : base(canvasGroup, animationDuration, ref isAnimating) {}
}

public class CrossfadeIn : Crossfade, IFadeIn
{
	public IEnumerator Play()
	{
		while (IsAnimating)
			yield return null;
		if (CrossfadeGroup.alpha > 0f)
		{
			IsAnimating = true;
			ActionList = InputSystem.ListEnabledActions();
			InputSystem.DisableAllEnabledActions();
			CrossfadeGroup.alpha = 1f;
			Time.timeScale = 0;
			float timer = 0f;
			while (timer < AnimationDuration)
			{
				float step = 1f / ((AnimationDuration - timer) / Time.unscaledDeltaTime);
				CrossfadeGroup.alpha -= step;
				yield return null;
				timer += Time.unscaledDeltaTime;
			}
			Time.timeScale = 1f;
			CrossfadeGroup.alpha = 0f;
			CrossfadeGroup.blocksRaycasts = false;
			CrossfadeGroup.interactable = false;
			foreach (InputAction action in ActionList)
				action.Enable();
			ActionList.Clear();
			IsAnimating = false;
		}
	}
	internal CrossfadeIn(CanvasGroup canvasGroup, float animationDuration, ref bool isAnimating) : base(canvasGroup, animationDuration, ref isAnimating) { }
}
