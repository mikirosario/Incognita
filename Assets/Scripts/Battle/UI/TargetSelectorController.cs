using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class TargetSelectorController : MonoBehaviour
{
	public enum Selectable
	{
		ENEMIES = 0,
		LIVE_ENEMIES,
		DEAD_ENEMIES,
		ALLIES,
		LIVE_ALLIES,
		DEAD_ALLIES,
		ALL,
		LIVE_ALL,
		DEAD_ALL
	}
	[SerializeField] private SpriteRenderer _selectorIcon;
	[SerializeField] private List<BattleRowBounds> _battleRows;
	BattleManager BattleManager => GameManager.Instance.BattleManager;
	PlayerInput PlayerInput => GameManager.Instance.PlayerInput;
	private SpriteRenderer SelectorIcon { get { return _selectorIcon; } set { _selectorIcon = value; } }
	private List<BattleRowBounds> BattleRows { get { return _battleRows; } }
	private Character SelectedCharacter { get; set; } = null;
	private List<Character> SelectableCharacters { get; set; } = new List<Character>(9);
	public bool IsSelectorActive { get { return SelectorIcon.gameObject.activeSelf; } }
	private void Awake()
	{
		BattleRows.Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
		PlayerInput.actions.FindActionMap("Battle").FindAction("Select").performed += OnSelect;
	}

	public void StartSelector(Selectable targets)
	{
		if (GetSelectableCharacters(targets) == false || SelectableCharacters.Count < 1)
			return;
		if (targets == Selectable.LIVE_ENEMIES || targets == Selectable.DEAD_ENEMIES || targets == Selectable.ENEMIES)
			SelectedCharacter = SelectableCharacters[SelectableCharacters.Count / 2];
		else
			SelectedCharacter = BattleManager.CurrentCharacter;
		SelectTarget(SelectedCharacter);
		SelectorIcon.gameObject.SetActive(true);
	}
	public void StopSelector()
	{
		DeselectTarget();
		SelectedCharacter = null;
		SelectableCharacters.Clear();
		SelectorIcon.gameObject.SetActive(false);
	}
	private bool GetSelectableCharacters(Selectable targets)
	{
		bool ret = true;
		Action sortCharactersVertically = () => SelectableCharacters.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
		switch (targets)
		{
			case Selectable.LIVE_ENEMIES:
				IEnumerable<Character> liveEnemies = BattleManager.EnemyParty.Where((a) => a.HitPointsCurrent > 0);
				SelectableCharacters.AddRange(liveEnemies);
				sortCharactersVertically();
				liveEnemies = null;
				break;
			default:
				ret = false;
				break;
		}
		return ret;
	}
	private void OnSelect(InputAction.CallbackContext context)
	{
		if (SelectedCharacter)
		{
			Vector2 inputVal = context.ReadValue<Vector2>();
			if (inputVal.y != 0f)
				SelectedCharacter = NextCharacterInRow(inputVal);
			SelectTarget(SelectedCharacter);
			//else if (inputVal == Vector2.right)

			//else
		}
	}
	private Character NextCharacterInRow(Vector2 inputVal)
	{
		BattleRowBounds currentRow = GetSelectedCharacterBattleRow();
		int posIndex = SelectableCharacters.FindIndex(a => a.transform.position == SelectedCharacter.transform.position);
		int addend;
		int mod = SelectableCharacters.Count;
		if (inputVal == Vector2.up)
			addend = 1;
		else
			addend = -1;
		do
		{
			posIndex = (((posIndex + addend) % mod) + mod) % mod;
		} while (!currentRow.Contains(SelectableCharacters[posIndex].transform));
		return SelectableCharacters[posIndex];

		// -		 -
		//	-		-
		// -		 -
		//if moving up, get closest x with greater y????
		//if moving down get closest x with lower y
		//if moving right get closest y with greater x
		//if moving left get closest y with lower x

	}
	private BattleRowBounds GetSelectedCharacterBattleRow()
	{
		BattleRowBounds row = BattleRows[0];
		foreach (BattleRowBounds battleRow in BattleRows)
		{
			row = battleRow;
			if (row.Contains(SelectedCharacter.transform))
				break;
		}
		return row;
	}
	private void SelectTarget(IPhysical target)
	{
		SelectorIcon.transform.position = new Vector3(target.Transform.position.x, target.Transform.position.y + 1f, target.Transform.position.z);
	}

	private void DeselectTarget()
	{
		SelectorIcon.transform.localPosition = Vector3.zero;
	}
}
