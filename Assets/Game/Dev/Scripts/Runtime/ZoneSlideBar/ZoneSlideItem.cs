using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VertigoGamesCaseStudy.Core.Utility;
using VertigoGamesCaseStudy.Runtime.Interfaces;

namespace VertigoGamesCaseStudy.Runtime.UI.ZoneSlide
{
	public class ZoneSlideItem : MonoBehaviour
	{
		[SerializeField] private Image currentZoneBgImage;
		[SerializeField] private TextMeshProUGUI currentZoneNumberText;

		private readonly IZoneColorProvider _zoneColorProvider = new ZoneColorProvider();

		public void UpdateCurrentZone(int zoneNumber, Sprite newZoneSwitchSprite, float switchDuration, bool animate = true)
		{
			currentZoneBgImage.sprite = newZoneSwitchSprite;
			currentZoneNumberText.text = $"{zoneNumber}";
			currentZoneNumberText.color = _zoneColorProvider.GetZoneNumberColor(zoneNumber, true);

			if(animate)
			{
				AnimateZoneSwitch(switchDuration);
			}
		}

		private void AnimateZoneSwitch(float switchDuration)
		{
			currentZoneBgImage.DOFillAmount(1, switchDuration).From(0).SetEase(Ease.InOutSine);
			currentZoneNumberText.transform.DOScale(Vector3.one, switchDuration).From(Vector3.zero).SetEase(Ease.InOutSine);
		}

	}
}