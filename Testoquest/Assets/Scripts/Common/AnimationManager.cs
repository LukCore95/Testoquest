using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
	[SerializeField] private Animator _playerAnimatorController;
	[SerializeField] private Animator _enemyAnimatorController;

	public void PlayIdleAnimation()
	{
		_playerAnimatorController.Play("Player_idle");
		_enemyAnimatorController.Play("Spider_idle");
	}

	public void PlayGoodAnswerAnimation()
	{
		if (Random.value >= 0.5)
		{
			_playerAnimatorController.SetTrigger("attack");
			_enemyAnimatorController.SetTrigger("hit");
		}
		else
		{
			_playerAnimatorController.SetTrigger("defense");
			_enemyAnimatorController.SetTrigger("attack");
		}
	}

	public void PlayBadAnswerAnimation()
	{
		if (Random.value >= 0.5)
		{
			_playerAnimatorController.SetTrigger("hit");
			_enemyAnimatorController.SetTrigger("attack");
		}
		else
		{
			_playerAnimatorController.SetTrigger("attack");
			_enemyAnimatorController.SetTrigger("defense");
		}
	}

	public void PlayWinAnimation()
	{
		_playerAnimatorController.SetTrigger("win");
		_enemyAnimatorController.SetTrigger("lose");
	}
}
