using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IScreenTransition
{
    IFadeOut FadeOut { get; }
    IFadeIn FadeIn { get; }
    bool IsAnimating { get; }
}

public interface IFadeOut
{
    IEnumerator Play();
    bool IsAnimating { get; }
}

public interface IFadeIn
{
    IEnumerator Play();
    bool IsAnimating { get; }
}

public class ScreenTransitionController : MonoBehaviour
{
    [SerializeField] private CrossfadeController _crossfade;
    public bool IsAnimating { get { return CrossfadeController.IsAnimating; } }
    public CrossfadeController CrossfadeController { get { return _crossfade; } }
}
