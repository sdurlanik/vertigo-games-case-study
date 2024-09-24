using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using VertigoGamesCaseStudy.Core.Utility;
using VertigoGamesCaseStudy.Runtime.Interfaces;

namespace VertigoGamesCaseStudy.Runtime.UI.ZoneSlide
{
	public class ZoneSlideBar : MonoBehaviour
	{
		[SerializeField] private RectTransform slideContent; // Content that holds the zone numbers (pivot of the content is on the left side of the first zone number)
		[SerializeField] private TextMeshProUGUI zoneNumberPrefab;
		[SerializeField] private int maxNumberCountInBar = 9;
		
		private float _stepPixel = 0;
		private readonly float _currentZoneBgWidth = 100;
		private readonly List<TextMeshProUGUI> _zoneNumbers = new List<TextMeshProUGUI>();
		private readonly IZoneColorProvider _zoneColorProvider = new ZoneColorProvider();

		int StartNumberCount => Mathf.CeilToInt((float) maxNumberCountInBar / 2);

		private void Start()
		{
			CalculateContentStartPosition();
			CreateInitialNumbers();
		}

		private void CalculateContentStartPosition()
		{
			var switchControllerWidth = GetComponent<RectTransform>().rect.width;
			_stepPixel = _currentZoneBgWidth;

			var slideContentStartPosition = switchControllerWidth / 2 - _currentZoneBgWidth / 2;
			slideContent.anchoredPosition = new Vector2(slideContentStartPosition, 0);
		}

		void CreateInitialNumbers()
		{
			for(int i = 0; i < StartNumberCount + 1; i++) // +1 extra number for the switch animation
			{
				AddZoneNumber(i + 1);
			}
		}

		public void SlideNumbers(int currentZoneNumber, float switchDuration)
		{
			slideContent.DOAnchorPosX(slideContent.anchoredPosition.x - _stepPixel, switchDuration)
				.SetEase(Ease.InOutSine)
				.OnComplete(() =>
				{
					RemoveOutOfSightNumbers();
					AddZoneNumber(currentZoneNumber + StartNumberCount);
				});
		}

		private void RemoveOutOfSightNumbers()
		{
			if(_zoneNumbers.Count <= maxNumberCountInBar) return;

			var firstNumber = _zoneNumbers[0];
			_zoneNumbers.RemoveAt(0);
			Destroy(firstNumber.gameObject);

			// we removed the first number, so we need to move the content to the right because of the pivot position
			slideContent.anchoredPosition = new Vector2(slideContent.anchoredPosition.x + _stepPixel, 0);
		}
		
		void AddZoneNumber(int zoneNumber)
		{
			var zoneNumberInstance = Instantiate(zoneNumberPrefab, slideContent);
			zoneNumberInstance.text = $"{zoneNumber}";
			zoneNumberInstance.color = _zoneColorProvider.GetZoneNumberColor(zoneNumber);
			zoneNumberInstance.gameObject.name = $"ZoneNumber_{zoneNumber}";
			
			_zoneNumbers.Add(zoneNumberInstance);
		}
		
		public void ResetSlideBar()
		{
			ClearNumbers();
			CalculateContentStartPosition();
			CreateInitialNumbers();
		}
		
		private void ClearNumbers()
		{
			foreach (var zoneNumber in _zoneNumbers)
			{
				Destroy(zoneNumber.gameObject);
			}
			
			_zoneNumbers.Clear();
		}
	}
}