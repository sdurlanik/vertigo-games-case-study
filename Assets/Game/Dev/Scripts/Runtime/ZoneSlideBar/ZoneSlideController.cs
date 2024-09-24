using UnityEngine;
using VertigoGamesCaseStudy.Core.Events;

namespace VertigoGamesCaseStudy.Runtime.UI.ZoneSlide
{
	public class ZoneSlideController : MonoBehaviour
	{
		[SerializeField] private float switchDuration = 0.5f;

		[SerializeField] private ZoneSlideItem currentZoneItem;
		[SerializeField] private ZoneSlideBar zoneSlideBar;


		private void OnEnable()
		{
			GameEventManager.Instance.On<StartNewZoneEvent>(OnNewZoneStarted);
			GameEventManager.Instance.On<GiveUpEvent>(OnGiveUp);
			GameEventManager.Instance.On<LeaveGameEvent>(OnLeaveGame);
		}

		private void OnDisable()
		{
			GameEventManager.Instance.Off<StartNewZoneEvent>(OnNewZoneStarted);
			GameEventManager.Instance.Off<GiveUpEvent>(OnGiveUp);
			GameEventManager.Instance.Off<LeaveGameEvent>(OnLeaveGame);
		}

		private void OnNewZoneStarted(StartNewZoneEvent startNewZoneEvent)
		{
			if(startNewZoneEvent.ZoneNumber == 1)
			{
				currentZoneItem.UpdateCurrentZone(startNewZoneEvent.ZoneNumber, startNewZoneEvent.Zone.switchBarSprite, 0, false);
				return;
			}

			currentZoneItem.UpdateCurrentZone(startNewZoneEvent.ZoneNumber, startNewZoneEvent.Zone.switchBarSprite, switchDuration);
			zoneSlideBar.SlideNumbers(startNewZoneEvent.ZoneNumber, switchDuration);
		}

		private void OnGiveUp(GiveUpEvent giveUpEvent)
		{
			zoneSlideBar.ResetSlideBar();
		}

		private void OnLeaveGame(LeaveGameEvent leaveGameEvent)
		{
			zoneSlideBar.ResetSlideBar();
		}
	}
}