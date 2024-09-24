using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Core.Events;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.ZoneInfo
{
    public class ZoneInfoController : MonoBehaviour
    {
       [SerializeField] private TextMeshProUGUI superZoneTargetLevelText;
       [SerializeField] private TextMeshProUGUI safeZoneTargetLevelText;
       [SerializeField] private Image specialRewardImage;
       
       [SerializeField] private SuperZoneSO superZoneSo;
	   [SerializeField] private SafeZoneSO safeZoneSo;

       private void OnEnable()
       {
	       GameEventManager.Instance.On<StartNewZoneEvent>(OnNewZoneStarted);
       }
       
       private void OnDisable()
	   {
	       GameEventManager.Instance.Off<StartNewZoneEvent>(OnNewZoneStarted);
	   }

	   private void OnNewZoneStarted(StartNewZoneEvent startNewZoneEvent)
	   {
		   var currentZoneNumber = startNewZoneEvent.ZoneNumber;
		   superZoneTargetLevelText.text = $"{CalculateNextZoneNumberByTriggerInterval(currentZoneNumber,superZoneSo.triggerInterval)}";
	       safeZoneTargetLevelText.text = $"{CalculateNextZoneNumberByTriggerInterval(currentZoneNumber,safeZoneSo.triggerInterval)}";
	       SetNextSpecialRewardImage(currentZoneNumber);
       }


	   private void SetNextSpecialRewardImage(int currentZoneNumber)
	   {
		   var specialRewards = superZoneSo.specialRewards;
		   var triggerInterval = superZoneSo.triggerInterval;

		   int rewardIndex = currentZoneNumber / triggerInterval;

		   if (rewardIndex >= 0 && rewardIndex < specialRewards.Count)
		   {
			   // Select the special reward image
			   var specialReward = specialRewards[rewardIndex];
			   specialRewardImage.sprite = specialReward.icon;
		   }
	   }

       private int CalculateNextZoneNumberByTriggerInterval(int currentZoneNumber, int triggerInterval)
       {
	       return ((currentZoneNumber / triggerInterval) + 1) * triggerInterval;
       }
    }
}
