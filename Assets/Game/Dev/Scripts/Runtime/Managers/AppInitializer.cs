using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.Managers
{
	public class AppInitializer : MonoBehaviour
	{
		[SerializeField] private RouletteManager rouletteManager;
		[SerializeField] private RewardManager rewardManager;
        
		void Awake()
		{
			rouletteManager.Initialize();
			rewardManager.Initialize();
		}
	}
}