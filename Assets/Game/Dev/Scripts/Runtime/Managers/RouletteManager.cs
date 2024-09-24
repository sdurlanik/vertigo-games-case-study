using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Core.Events;
using VertigoGamesCaseStudy.Runtime.Interfaces;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;
using VertigoGamesCaseStudy.Runtime.UI.Roulette;

namespace VertigoGamesCaseStudy.Runtime.Managers
{
    public class RouletteManager : BaseManager
    {
        [SerializeField] private Image rouletteBaseImage;
        [SerializeField] private Image rouletteIndicatorImage;
        [SerializeField] private Button spinButton;
        [SerializeField] private Transform spinRoot, slotsRoot;
        [SerializeField] private SlotItem slotItemPrefab;
        [SerializeField] private RewardSO bombReward; 
        
        [SerializeField] private float spinDuration = 3f;
        [SerializeField] private float rotationAngle = 360f;
        [SerializeField] private int numberOfSpins = 5;
        
        private float _currentRewardMultiplier = 1f;
        private int _currentZoneNumber = 1;
        private IRewardSelector _rewardSelector;
        private ZoneBase _currentZone;
        private SpinAnimator _spinAnimator;
        private RouletteSlotController _rouletteSlotController;
        private List<RewardSO> _selectedRewards = new List<RewardSO>();

        private readonly int _sliceCount = 8;
        
        private void OnEnable()
        {
            GameEventManager.Instance.On<StartNewZoneEvent>(OnStartNewZone);
            spinButton.onClick.AddListener(SpinRoulette);
        }

        private void OnDisable()
        {
            GameEventManager.Instance.Off<StartNewZoneEvent>(OnStartNewZone);
            spinButton.onClick.RemoveListener(SpinRoulette);
        }

       

        private void Start()
        {
            spinButton.interactable = false;
        }

        private void OnValidate()
        {
            if(spinButton == null)
            {
                spinButton = GetComponentInChildren<Button>();
            }
        }
        
        public override void Initialize()
        {
            _rewardSelector = new RewardSelector();
            _spinAnimator = new SpinAnimator(spinRoot, spinDuration, rotationAngle, numberOfSpins, _sliceCount);
            _rouletteSlotController = new RouletteSlotController(slotsRoot, slotItemPrefab);
        }
        
        private void OnStartNewZone(StartNewZoneEvent startNewZoneEvent)
        {
            ResetRoulette();
            SetRoulette(startNewZoneEvent.Zone, startNewZoneEvent.ZoneNumber);
            SetSlots(_currentZone.rewards);
        }
        
        private void SetRoulette(ZoneBase zone, int zoneNumber)
        {
            _currentZone = zone;
            _currentZoneNumber = zoneNumber;
            _currentRewardMultiplier = _currentZone.rewardValueMultiplier * zoneNumber;
            
            rouletteBaseImage.sprite = _currentZone.rouletteBaseSprite;
            rouletteIndicatorImage.sprite = _currentZone.rouletteIndicatorSprite;
            spinButton.interactable = false;
        }

        private void SpinRoulette()
        {
            spinButton.interactable = false;

            var selectedReward = _rewardSelector.SelectReward(_selectedRewards, _currentZoneNumber);
            var selectedIndex = _selectedRewards.IndexOf(selectedReward);

            _spinAnimator.SpinToIndex(selectedIndex, () => OnSpinComplete(selectedReward));
            
            var rouletteSpinStartEvent = new RouletteSpinStartEvent();
            GameEventManager.Instance.Fire(rouletteSpinStartEvent);
        }

        private void OnSpinComplete(RewardSO selectedReward)
        {
            Debug.Log("Selected Reward: " + selectedReward.name);

            if(selectedReward.isBomb)
            {
                var bombExplodedEvent = new BombExplodedEvent();
                GameEventManager.Instance.Fire(bombExplodedEvent);
                return;
            }

            var slotItem = _rouletteSlotController.GetSlotItemByRewardSo(selectedReward);
            var rouletteSpinEndEvent = new RouletteSpinEndEvent(slotItem, _currentRewardMultiplier);
            
            GameEventManager.Instance.Fire(rouletteSpinEndEvent);
        }
        private void SetSlots(List<RewardSO> rewards)
        {
            var isDefaultZone = _currentZone is DefaultZoneSO;
            var rewardCount = isDefaultZone ? _sliceCount - 1 : _sliceCount; 
            _selectedRewards = _rewardSelector.SelectRewards(rewards, rewardCount, _currentZoneNumber);

            if(isDefaultZone)
            {
                var randomIndex = UnityEngine.Random.Range(0, _sliceCount);
                _selectedRewards.Insert(randomIndex, bombReward);
            }
            
            StartCoroutine(InstantiateRewardItemsCoroutine());
        }

        private IEnumerator InstantiateRewardItemsCoroutine()
        {
            yield return StartCoroutine(_rouletteSlotController.InstantiateSlotItems(_selectedRewards, _currentRewardMultiplier, _sliceCount));
            spinButton.interactable = true;
        }

        private void ResetRoulette()
        {
            _currentRewardMultiplier = 1f;
            _selectedRewards.Clear();
            _rouletteSlotController.ResetSlots();
        }
    }
}