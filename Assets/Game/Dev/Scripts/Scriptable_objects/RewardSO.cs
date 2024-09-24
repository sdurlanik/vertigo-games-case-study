using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.ScriptableObjects
{
	[CreateAssetMenu (fileName = "Reward", menuName = "VertigoGamesCaseStudy/Reward")]
	public class RewardSO : ScriptableObject
	{
		public string displayName;
		public Sprite icon;
		public bool isBomb;
		[SerializeField] private int baseAmount;
		
		[Range(1, 100), Tooltip("Lower values represents the chance of this reward to be won")]
		[SerializeField] private float weight;
		[SerializeField] private float weightMultiplier = 1.03f;
		
		public float CalculateWeight(int zoneNumber)
		{
			return weight * Mathf.Pow(weightMultiplier, zoneNumber);
		}
		
		public int CalculateRewardAmount(float multiplier)
		{
			return Mathf.RoundToInt(baseAmount + baseAmount * multiplier);
		}
	}
}
