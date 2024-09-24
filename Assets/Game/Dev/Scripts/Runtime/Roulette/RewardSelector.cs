using System;
using System.Collections.Generic;
using VertigoGamesCaseStudy.Runtime.Interfaces;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.Roulette
{
    public class RewardSelector : IRewardSelector
    {
        public List<RewardSO> SelectRewards(List<RewardSO> availableRewards, int count, int zoneNumber)
        {
            if (availableRewards == null || availableRewards.Count == 0)
                throw new ArgumentException("availableRewards cannot be null or empty.");

            // Precompute cumulative weights once
            var cumulativeWeights = ComputeCumulativeWeights(availableRewards, zoneNumber, out float totalWeight);

            var selectedRewards = new List<RewardSO>();

            for (int i = 0; i < count; i++)
            {
                var randomValue = UnityEngine.Random.Range(0f, totalWeight);
                var index = GetIndexFromCumulativeWeights(cumulativeWeights, randomValue);
                selectedRewards.Add(availableRewards[index]);
            }

            return selectedRewards;
        }

        public RewardSO SelectReward(List<RewardSO> rewards, int zoneNumber)
        {
            if (rewards == null || rewards.Count == 0)
                throw new ArgumentException("rewards cannot be null or empty.");
            
            var cumulativeWeights = ComputeCumulativeWeights(rewards, zoneNumber, out var totalWeight);

            var randomValue = UnityEngine.Random.Range(0f, totalWeight);
            var index = GetIndexFromCumulativeWeights(cumulativeWeights, randomValue);
            return rewards[index];
        }
        
        private float[] ComputeCumulativeWeights(List<RewardSO> rewards, int zoneNumber, out float totalWeight)
        {
            var count = rewards.Count;
            var cumulativeWeights = new float[count];
            var cumulativeWeight = 0f;

            for (int i = 0; i < count; i++)
            {
                cumulativeWeight += rewards[i].CalculateWeight(zoneNumber);
                cumulativeWeights[i] = cumulativeWeight;
            }

            totalWeight = cumulativeWeight;
            return cumulativeWeights;
        }

        private int GetIndexFromCumulativeWeights(float[] cumulativeWeights, float randomValue)
        {
            // binary search for efficiency
            var index = Array.BinarySearch(cumulativeWeights, randomValue);
            if (index < 0)
            {
                // If not found, BinarySearch returns the bitwise complement of the next larger element
                index = ~index;
            }
            return index;
        }
    }
}
