using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Core.Events;
using VertigoGamesCaseStudy.Runtime.Managers;
using VertigoGamesCaseStudy.Runtime.UI.Roulette;

namespace VertigoGamesCaseStudy.Runtime.UI.Popup
{
    public class RewardPopup : PopupBase
    {
        [SerializeField] private RewardManager rewardManager;
        [SerializeField] private Image rewardImage;
        [SerializeField] private TextMeshProUGUI rewardNameText, rewardAmountText;
        [SerializeField] private int cloneCount = 5;
        [SerializeField] private float expandDuration = 0.5f;
        [SerializeField] private float moveDuration = 0.7f;
        
        private readonly Vector3 _initialScale = Vector3.zero;
        private readonly Vector3 _targetScale = Vector3.one;
        private const float ROTATION_AMOUNT = 360f;

        private SlotItem _slotItem;
        private readonly List<Image> _rewardImageClones = new List<Image>(); // Store the clones for simultaneous movement
        
        public void SetReward(SlotItem rewardItem, float rewardMultiplier)
        {
            ResetValues();

            _slotItem = rewardItem;
            rewardImage.sprite = rewardItem.Reward.icon;
            rewardNameText.text = rewardItem.Reward.displayName;
            rewardAmountText.text = $"x{Mathf.RoundToInt(rewardItem.Reward.CalculateRewardAmount(rewardMultiplier))}";
        }

        protected override void Enter()
        {
            base.Enter();
            Debug.Log("RewardPopup Enter");
            PlayEnterAnimation();
        }

        private void ResetValues()
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = _initialScale;
            _rewardImageClones.Clear();
            _slotItem = null;
        }

        private void PlayEnterAnimation()
        {
            AnimateScale(_targetScale, expandDuration, Ease.OutBack);
            AnimateRotation(ROTATION_AMOUNT, expandDuration, Ease.OutCubic, StartRewardAnimation);
        }

        private void AnimateScale(Vector3 scaleTarget, float duration, Ease easeType)
        {
            transform.DOScale(scaleTarget, duration).SetEase(easeType);
        }

        private void AnimateRotation(float angle, float duration, Ease easeType, TweenCallback onComplete)
        {
            transform.DORotate(new Vector3(0, 0, angle), duration, RotateMode.FastBeyond360)
                .SetEase(easeType)
                .OnComplete(onComplete);
        }
        
        private void StartRewardAnimation()
        {
            for(int i = 0; i < cloneCount; i++)
            {
                StartCoroutine(CreateCloneAndExpand(i));
            }
        }

        private IEnumerator CreateCloneAndExpand(int index)
        {
            yield return new WaitForSeconds(index * 0.1f); 
            var rewardImageClone = CreateCloneImage();
            _rewardImageClones.Add(rewardImageClone); 

            PlayCloneExpandAnimation(rewardImageClone);
            
            if(index == cloneCount - 1)
            {
                yield return new WaitForSeconds(expandDuration);
                MoveAllClonesToTarget();
            }
        }

        private void PlayCloneExpandAnimation(Image clone)
        {
            var randomOffset = (Vector3)Random.insideUnitCircle * 100f; // Create a random offset for expansion
            var expandedPosition = clone.transform.localPosition + Vector3.up * 100 + randomOffset;

            clone.transform.DOScale(Vector3.one * 0.5f, expandDuration)
                .SetEase(Ease.OutBack);

            clone.transform.DOLocalMove(expandedPosition, expandDuration)
                .SetEase(Ease.OutBack);
        }

        private void MoveAllClonesToTarget()
        {
            foreach(Image clone in _rewardImageClones)
            {
                clone.transform.DOMove(rewardManager.GetNextRewardItemPosition(_slotItem), moveDuration)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        Destroy(clone.gameObject);
                    });
            }

            DOVirtual.DelayedCall(moveDuration, Exit);
        }
        private Image CreateCloneImage()
        {
            var clone = Instantiate(rewardImage, rewardImage.transform.position, Quaternion.identity, transform);
            clone.transform.localScale = _initialScale;
            return clone;
        }
        protected override void Exit()
        {
            base.Exit();
            PlayExitAnimation();
            FireRewardPopupExitEvent();
        }

        private void PlayExitAnimation()
        {
            var targetPosition = new Vector3(0, -Screen.height, 0);
            var duration = 1f;

            transform.DOLocalMove(targetPosition, duration).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
            AnimateScale(_initialScale, duration, Ease.OutBack);
        }

        private void FireRewardPopupExitEvent()
        {
            var rewardPopupExitEvent = new RewardPopupExitEvent(_slotItem);
            GameEventManager.Instance.Fire(rewardPopupExitEvent);
        }

    }
}
