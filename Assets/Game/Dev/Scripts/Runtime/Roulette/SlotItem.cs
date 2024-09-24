using UnityEngine;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.Roulette
{
	public class SlotItem : BaseItem
	{
		public void SetItem(RewardSO reward, int slotIndex,float rewardMultiplier, int sliceCount)
		{
			SetRotationByIndex(sliceCount,slotIndex);
			
			RewardAmount = Mathf.RoundToInt(reward.CalculateRewardAmount(rewardMultiplier));
			Reward = reward;
			iconImage.sprite = reward.icon;
			
			if(!reward.isBomb)
				amountText.text = $"x{RewardAmount}";
			else
				amountText.gameObject.SetActive(false);
			
			gameObject.name = $" slot_{slotIndex}_{reward.displayName}";
		}

	
		private void SetRotationByIndex(int sliceCount, int index)
		{
			var rectTransform = GetComponent<RectTransform>();
			
			var angle = 360f / sliceCount * index;
			rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
		}
	}
}