using System.Collections.Generic;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.Interfaces
{
	public interface IRewardSelector
	{
		List<RewardSO> SelectRewards(List<RewardSO> availableRewards, int count, int zoneNumber);
		RewardSO SelectReward(List<RewardSO> rewards, int zoneNumber);
	}
}