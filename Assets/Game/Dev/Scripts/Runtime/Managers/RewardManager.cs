using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Core.Events;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;
using VertigoGamesCaseStudy.Runtime.UI.Roulette;

namespace VertigoGamesCaseStudy.Runtime.Managers
{
    public class RewardManager : BaseManager
    {
        [SerializeField] private RewardItem rewardItemPrefab;
        [SerializeField] private RectTransform rewardItemRoot;
        [SerializeField] private Button leaveGameButton;

        private readonly Dictionary<RewardSO, RewardItem> _rewardItemDict = new Dictionary<RewardSO, RewardItem>();
        private float _rewardItemHeight;
        
        private void OnEnable()
        {
            GameEventManager.Instance.On<RewardPopupExitEvent>(OnRewardPopupExit);
            GameEventManager.Instance.On<RouletteSpinStartEvent>(OnRouletteSpinStart);
            GameEventManager.Instance.On<GiveUpEvent>(OnGiveUp);
            GameEventManager.Instance.On<ReviveEvent>(OnRevive);
            
            leaveGameButton.onClick.AddListener(LeaveGame);
        }

       
        private void OnDisable()
        {
            GameEventManager.Instance.Off<RewardPopupExitEvent>(OnRewardPopupExit);
            GameEventManager.Instance.Off<RouletteSpinStartEvent>(OnRouletteSpinStart);
            GameEventManager.Instance.Off<GiveUpEvent>(OnGiveUp);
            GameEventManager.Instance.Off<ReviveEvent>(OnRevive);
            leaveGameButton.onClick.RemoveListener(LeaveGame);
        }

   

        private void OnValidate()
        {
            if (leaveGameButton == null)
            {
                leaveGameButton = GetComponentInChildren<Button>();
            }
        }
        
        public override void Initialize()
        {
            var prefabRectTransform = rewardItemPrefab.GetComponent<RectTransform>();
            _rewardItemHeight = prefabRectTransform.rect.height;
            
            leaveGameButton.interactable = false;
        }
        
        public Vector3 GetNextRewardItemPosition(SlotItem newReward)
        {
            if (newReward == null || newReward.Reward == null)
            {
                return Vector3.zero;
            }

            if (_rewardItemDict.TryGetValue(newReward.Reward, out var existingRewardItem))
            {
                return existingRewardItem.transform.position;
            }
            else
            {
                if(_rewardItemDict.Count == 0)
                {
                    var topPositionY = rewardItemRoot.position.y + rewardItemRoot.rect.height / 2;
                    var newY = topPositionY - _rewardItemHeight / 2;
                    return new Vector3(rewardItemRoot.position.x, newY, rewardItemRoot.position.z);
                }

                var lastRewardSo = _rewardItemDict.Last().Key;
                if(_rewardItemDict.TryGetValue(lastRewardSo, out var lastRewardItem))
                {
                    var newY = lastRewardItem.transform.position.y - _rewardItemHeight;
                    return new Vector3(lastRewardItem.transform.position.x, newY, lastRewardItem.transform.position.z);
                }
                else
                {
                    return rewardItemRoot.position;
                }
            }
        }

        private void OnRewardPopupExit(RewardPopupExitEvent rewardPopupExitEvent)
        {
            AddRewardItem(rewardPopupExitEvent.SlotItem);
            leaveGameButton.interactable = true;
        }
        private void OnRouletteSpinStart(RouletteSpinStartEvent e)
        {
            leaveGameButton.interactable = false;
        }

        private void OnRevive(ReviveEvent reviveEvent)
        {
            leaveGameButton.interactable = true;
        }

        private void OnGiveUp(GiveUpEvent giveUpEvent)
        {
            ClearRewardItems();
        }

        private void ClearRewardItems()
        {
            foreach (var rewardItem in _rewardItemDict.Values)
            {
                if (rewardItem != null)
                {
                    Destroy(rewardItem.gameObject);
                }
            }

            _rewardItemDict.Clear();
        }

        private void AddRewardItem(SlotItem newReward)
        {
            if (newReward == null || newReward.Reward == null)
            {
                return;
            }

            if (_rewardItemDict.TryGetValue(newReward.Reward, out RewardItem existingRewardItem))
            {
                existingRewardItem.IncreaseRewardValue(newReward.RewardAmount);
            }
            else
            {
                var rewardItemInstance = Instantiate(rewardItemPrefab, rewardItemRoot);
                rewardItemInstance.SetItem(newReward.Reward, newReward.RewardAmount);
                
                _rewardItemDict.Add(newReward.Reward, rewardItemInstance);
            }
        }

        

        private void LeaveGame()
        {
            ClearRewardItems();
            
            var leaveGameEvent = new LeaveGameEvent();
            GameEventManager.Instance.Fire(leaveGameEvent);
        }
    }
}
