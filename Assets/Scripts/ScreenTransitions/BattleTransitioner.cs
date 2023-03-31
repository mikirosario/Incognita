using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class BattleTransitioner : MonoBehaviour
{
	private ScreenTransitionController TransitionController => GameManager.Instance.ScreenTransitionController;
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (GameManager.Instance.ActiveScene.Equals(GameManager.SceneIndex.ExplorationScene) && collision.gameObject.CompareTag("Player"))
			//GameManager.Instance.SetActiveScene(GameManager.SceneIndex.BattleScene, GameManager.Instance.ExplorationManager.BattleArea);
			GameManager.Instance.TransitionActiveScene(TransitionController.CrossfadeController.FadeOut, TransitionController.CrossfadeController.FadeIn, GameManager.SceneIndex.BattleScene, GameManager.Instance.ExplorationManager.BattleArea);
	}
}
