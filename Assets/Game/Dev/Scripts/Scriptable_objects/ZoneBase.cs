using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.ScriptableObjects
{
	
	public abstract class ZoneBase : ScriptableObject
	{
		[Header("Roulette Variables")]
		public Sprite rouletteBaseSprite;
		public Sprite rouletteIndicatorSprite;
		
		[Header("Switch Bar Variables")]
		public Sprite switchBarSprite;
		
		[Header("Rewards")]
		public List<RewardSO> rewards;
		
		[Header("Zone Settings"), Tooltip("this will trigger the safe zone every x levels")]
		public int triggerInterval = 5;
		
		[Tooltip("this will multiply the reward value by x")]
		public float rewardValueMultiplier = 0.01f;
	}
}