using System;
using UnityEngine;
using VertigoGamesCaseStudy.Core.Events;
using VertigoGamesCaseStudy.Runtime.Interfaces;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;
using VertigoGamesCaseStudy.Runtime.UI.ZoneSlide;

namespace VertigoGamesCaseStudy.Runtime.Managers
{
	public class ZoneManager : MonoBehaviour
	{
		[Header("Zone Data")]
		[SerializeField] private SuperZoneSO superZoneSo;
		[SerializeField] private SafeZoneSO safeZoneSo;
		[SerializeField] private DefaultZoneSO defaultZoneSo;
		
		private int _currentZoneNumber = 0;
		private IZoneSelector _zoneSelector;

		private void Awake()
		{
			_zoneSelector = new ZoneSelector(superZoneSo, safeZoneSo, defaultZoneSo);
		}

		private void Start()
		{
			StartNewZone();
		}

		private void OnEnable()
		{
			GameEventManager.Instance.On<RewardPopupExitEvent>(OnRewardPopupExit);
			GameEventManager.Instance.On<GiveUpEvent>(OnGiveUp);
			GameEventManager.Instance.On<ReviveEvent>(OnRevive);
			GameEventManager.Instance.On<LeaveGameEvent>(OnLeaveGame);
		}

	

		private void OnDisable()
		{
			GameEventManager.Instance.Off<RewardPopupExitEvent>(OnRewardPopupExit);
			GameEventManager.Instance.Off<GiveUpEvent>(OnGiveUp);
			GameEventManager.Instance.Off<ReviveEvent>(OnRevive);
			GameEventManager.Instance.Off<LeaveGameEvent>(OnLeaveGame);
		}

		

		private void OnRevive(ReviveEvent reviveEvent)
		{
			StartNewZone();
		}

		private void OnGiveUp(GiveUpEvent giveUpEvent)
		{
			_currentZoneNumber = 0;
			StartNewZone();
		}
		
		private void OnLeaveGame(LeaveGameEvent leaveGameEvent)
		{
			_currentZoneNumber = 0;
			StartNewZone();
		}

		private void OnRewardPopupExit(RewardPopupExitEvent rewardPopupExitEvent)
		{
			StartNewZone();
		}
		
		private void StartNewZone()
		{
			_currentZoneNumber++;
			
			var currentZone = _zoneSelector.GetCurrentZone(_currentZoneNumber);
			var startNewZoneEvent = new StartNewZoneEvent(_currentZoneNumber, currentZone);
			
			GameEventManager.Instance.Fire(startNewZoneEvent);
		}
	}
}
