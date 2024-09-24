using DG.Tweening;
using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.UI.Popup
{
	public class LeaveGamePopup : PopupBase
	{
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform contentPanel;
        
        [Header("Animation Settings")]
        [SerializeField] private float fadeDuration = .2f;
        [SerializeField] private float contentPanelAnimationDuration = .4f;
        [SerializeField] private float exitTime = 1.5f;

        protected override void Enter()
        {
	        base.Enter();
	        AnimatePopupEnter();
	        DOVirtual.DelayedCall(exitTime, Exit);
        }

        private void AnimatePopupEnter()
        {
	       canvasGroup.DOFade(1, fadeDuration).From(0);
	       contentPanel.DOScale(Vector3.one, contentPanelAnimationDuration).From(Vector3.zero).SetEase(Ease.OutBack);
        }
        
        protected override void Exit()
		{
			base.Exit();
			AnimatePopupExit();
		}

		private void AnimatePopupExit()
		{
			var exitSequence = DOTween.Sequence();

			exitSequence.Append(contentPanel.DOScale(Vector3.zero, contentPanelAnimationDuration).SetEase(Ease.InBack));
			exitSequence.Append(canvasGroup.DOFade(0, fadeDuration));
			exitSequence.OnComplete(() => gameObject.SetActive(false));
		}

	}
}