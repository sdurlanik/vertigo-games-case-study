using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.Roulette
{
	public class RewardItem : BaseItem
	{
		public void SetItem(RewardSO reward, int rewardAmount)
		{
			Reward = reward;
			RewardAmount = rewardAmount;
			iconImage.sprite = reward.icon;
			
			UpdateAmount();
		}

		public void IncreaseRewardValue(int newValue)
		{
			RewardAmount += newValue;
			UpdateAmount();
			PlayScaleUpDownAnimation();
		}

		private void UpdateAmount()
		{
			var currentAmount = int.Parse(amountText.text.Replace("x", ""));

			DOTween.To(() => currentAmount, x => currentAmount = x, RewardAmount, 0.5f)
				.OnUpdate(() =>
				{
					amountText.text = $"x{currentAmount}";
				})
				.SetEase(Ease.OutCubic); 
		}
		private void PlayScaleUpDownAnimation()
		{
			var rectTransform = GetComponent<RectTransform>();
			rectTransform.localScale = Vector3.one;

			rectTransform.DOScale(Vector3.one * 1.2f, scaleAnimationDuration)
				.SetEase(Ease.OutBack)
				.OnComplete(() => rectTransform.DOScale(Vector3.one, scaleAnimationDuration));
		}
	}
}
