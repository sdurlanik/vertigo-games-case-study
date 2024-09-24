using System.Collections.Generic;
using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.ScriptableObjects
{
	[CreateAssetMenu (fileName = "SpecialZone", menuName = "VertigoGamesCaseStudy/SpecialZone")]
	public class SuperZoneSO : ZoneBase
	{
		public List<RewardSO> specialRewards;
		
		public RewardSO GetSpecialRewardByIndex(int index)
		{
			return specialRewards[index];
		}
	}
}