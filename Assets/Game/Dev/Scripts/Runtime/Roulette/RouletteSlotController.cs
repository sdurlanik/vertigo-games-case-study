using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.Roulette
{
	public class RouletteSlotController
	{
		private readonly Transform _slotsRoot;
		private readonly SlotItem _slotItemPrefab;
		private readonly List<SlotItem> _slotItems = new List<SlotItem>();

		public RouletteSlotController(Transform slotsRoot, SlotItem slotItemPrefab)
		{
			_slotsRoot = slotsRoot;
			_slotItemPrefab = slotItemPrefab;
		}

		public IEnumerator InstantiateSlotItems(List<RewardSO> selectedRewards, float rewardMultiplier, int sliceCount)
		{
			for (int i = 0; i < selectedRewards.Count; i++)
			{
				var rewardSo = selectedRewards[i];
				var slotItem = Object.Instantiate(_slotItemPrefab, _slotsRoot);
				slotItem.SetItem(rewardSo, i, rewardMultiplier, sliceCount);
				_slotItems.Add(slotItem);
				yield return new WaitForSeconds(0.05f);
			}
		}

		public void ResetSlots()
		{
			foreach (Transform child in _slotsRoot)
			{
				Object.Destroy(child.gameObject);
			}
			_slotItems.Clear();
		}

		public SlotItem GetSlotItemByRewardSo(RewardSO selectedReward)
		{
			return _slotItems.Find(item => item.Reward == selectedReward);
		}
	}
}