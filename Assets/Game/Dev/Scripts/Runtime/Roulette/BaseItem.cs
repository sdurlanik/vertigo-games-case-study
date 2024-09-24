using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.Roulette
{
	public class BaseItem : MonoBehaviour
	{
		public int RewardAmount { get; set; } 
		public RewardSO Reward { get; set; }
		
		[SerializeField] protected TextMeshProUGUI amountText;
		[SerializeField] protected Image iconImage;
		
		[SerializeField] protected float scaleAnimationDuration = 0.3f;
		
		protected virtual void OnEnable()
		{
			PlayScaleAnimation();
		}
		
		private void PlayScaleAnimation()
		{
			// Play scale-up animation and then scale-down back to normal
			var rectTransform = GetComponent<RectTransform>();
			rectTransform.localScale = Vector3.zero; // Start with zero scale

			// Scale up to max scale and then back down to normal (1, 1, 1)
			rectTransform.DOScale(Vector3.one, scaleAnimationDuration)
				.SetEase(Ease.OutBack);
		}
	}
}