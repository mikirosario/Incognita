using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleActionPanelController : MonoBehaviour
{
    private bool _isAnimating = false;
    [SerializeField] private float _extendedWidth;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Toggle[] _toggles;
    [Range(0f, 1f), SerializeField] private float _speed;
    private ActionPanelSettings CurrentPanelSettings { get; set; }
    private delegate bool TargetReached();

    private Animator Animator { get; set; }
    private bool IsAnimating { get { return _isAnimating; } set { _isAnimating = value; } }
    private float ExtendedWidth { get { return QuickRound(_extendedWidth); } }
    private float CurrentWidth { get { return QuickRound(RectTransform.sizeDelta.x); } }
    private Toggle[] Toggles { get { return _toggles; } }
    private float Speed { get { return _speed; } set { _speed = Mathf.Clamp(value, 0f, 1f); } }
    private RectTransform RectTransform { get { return _rectTransform; } }
    private void Awake()
	{
        Animator = GetComponent<Animator>();
	}

    private float QuickRound(float num)
	{
        return Mathf.Round(num * 100f) * 0.01f;
	}

    private void DisableToggles()
	{
        foreach (Toggle toggle in Toggles)
		{
            toggle.isOn = false;
            toggle.enabled = false;
		}
	}

    private void EnableToggles()
	{
        foreach (Toggle toggle in Toggles)
            toggle.enabled = true;
	}

    private IEnumerator ActionPanelSlideToggle()
	{
        if (IsAnimating == false)
        {
            IsAnimating = true;
            bool isDone = false;
            float targetWidth = Mathf.Abs(CurrentWidth - ExtendedWidth);
            float time = 0.0f;
            bool isSlidingOut = !(targetWidth == 0f);
            TargetReached targetReached;// = targetWidth == 0f ? () => { return CurrentWidth <= targetWidth + 0.1f; } : () => { return CurrentWidth >= targetWidth - 0.1f; };
            if (!isSlidingOut)
            {
                targetReached = () => { return CurrentWidth <= targetWidth + 0.1f; };
                DisableToggles();
            }
            else
            {
                targetReached = () => { return CurrentWidth >= targetWidth - 0.1f; };
                EnableToggles();
            }
            while (isDone == false)
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.Lerp(CurrentWidth, targetWidth, time));
                time += Speed * Time.deltaTime;
                yield return null;
                if (targetReached())
                {
                    RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                    isDone = true;
                }
            }
            IsAnimating = false;
        }
    }

    public void ActionPanelSlide()
	{
		if (IsAnimating == false)
			StartCoroutine(ActionPanelSlideToggle());
		//      if (IsAnimating == false)
		//{
		//          IsAnimating = true;
		//          Animator.Play("ActionPanelSlideOut");
		//}
	}

    // Start is called before the first frame update
    void Start()
    {
    }

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Return))
            ActionPanelSlide();
	}

    private class ActionPanelSettings
	{
        internal ICharacter Character { get; private set; }
        internal Color CharacterColor { get { return Character.Color; } }
        //character-specific OnClick() methods to use in buttons

        internal ActionPanelSettings(ICharacter character)
		{
            Character = character;
		}
	}
}
