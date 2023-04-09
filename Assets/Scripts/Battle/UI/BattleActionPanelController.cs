using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleActionPanelController : MonoBehaviour
{
    [SerializeField] private StatusMenuController _statusMenuController; //defer to BattleManager or GameManager singleton?

    private bool _isAnimating = false;
    [SerializeField] private float _extendedWidth;
    [SerializeField] private float _shiftSlotPositionBy;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Toggle[] _toggles;
    [Range(0f, 1f), SerializeField] private float _slideDuration;
    
    private ActionPanelSettings CurrentPanelSettings { get; set; }
    private BottomRowController BottomRowController { get; set; }
    private delegate bool TargetReached();
    private bool IsAnimating { get { return _isAnimating; } set { _isAnimating = value; } }
    private float ExtendedWidth { get { return QuickRound(_extendedWidth); } }
    private float CurrentWidth { get { return QuickRound(RectTransform.sizeDelta.x); } }
    private float ShiftSlotPositionBy { get { return _shiftSlotPositionBy; } }
    private Toggle[] Toggles { get { return _toggles; } }
    private float SlideDuration { get { return _slideDuration; } set { _slideDuration = Mathf.Clamp(value, 0f, 1f); } }
    private RectTransform RectTransform { get { return _rectTransform; } }
    private void Awake()
	{
        BottomRowController = _statusMenuController.BottomRowController;
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



    private IEnumerator ActionPanelSlideToggle(PlayerSlotController playerSlot)
	{
        IsAnimating = true;
        bool isDone = false;
        float targetWidth = Mathf.Abs(CurrentWidth - ExtendedWidth);
        bool isSlidingOut = !(targetWidth == 0f);
        float currentVelocity = 0.0f;
        TargetReached targetReached;
        if (!isSlidingOut)
        {
            targetReached = () => { return CurrentWidth <= targetWidth + 1f; };
            DisableToggles();
        }
        else
        {
            targetReached = () => { return CurrentWidth >= targetWidth - 1f; };
            //change background color to player color
            EnableToggles();
        }
        StartCoroutine(PlayerSlotSlideToggle(isSlidingOut, playerSlot));
        while (isDone == false)
        {
            RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.SmoothDamp(CurrentWidth, targetWidth, ref currentVelocity, SlideDuration));
            yield return null;
            if (targetReached())
            {
                RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
                isDone = true;
            }
        }
        IsAnimating = false;
    }

	private IEnumerator PlayerSlotSlideToggle(bool isSlidingOut, PlayerSlotController playerSlot)
	{
        bool isDone = false;
        //PlayerSlotController playerSlot = _statusMenuController.BottomRowController.PlayerSlot0; //get selected player slot
        float targetDiff = isSlidingOut == true ? ShiftSlotPositionBy * -1f : ShiftSlotPositionBy;
        float targetPos = playerSlot.transform.localPosition.x + targetDiff;
        float currentVelocity = 0.0f;
        TargetReached targetReached;
        if (!isSlidingOut)
		{
            targetReached = () => { return playerSlot.transform.localPosition.x >= targetPos - 1f; };
            playerSlot.TextObject.color = Color.white;
            //change color to white
		}
        else
		{
            targetReached = () => { return playerSlot.transform.localPosition.x <= targetPos + 1f; };
            playerSlot.TextObject.color = playerSlot.Character.Color;
            //change color to player color;
		}
        while (isDone == false)
		{
            playerSlot.transform.localPosition = new Vector2(Mathf.SmoothDamp(playerSlot.transform.localPosition.x, targetPos, ref currentVelocity, SlideDuration), playerSlot.transform.localPosition.y);
            yield return null;
            if (targetReached())
			{
                playerSlot.transform.localPosition = new Vector2(targetPos, playerSlot.transform.localPosition.y);
                isDone = true;
			}
		}
	}

	public void ActionPanelSlide(PlayerSlotController playerSlot)
	{
		if (IsAnimating == false)
			StartCoroutine(ActionPanelSlideToggle(playerSlot));
	}

	private void Update()
	{
        ////Activate player slot
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    ActionPanelSlide();
        //}
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
