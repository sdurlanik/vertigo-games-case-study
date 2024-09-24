using DG.Tweening;
using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.UI.Roulette
{
	public class SpinAnimator
	{
		private readonly Transform _spinRoot;
		private readonly float _spinDuration;
		private readonly float _rotationAngle;
		private readonly int _numberOfSpins;
		private readonly int _sliceCount;

		public SpinAnimator(Transform spinRoot, float spinDuration, float rotationAngle, int numberOfSpins, int sliceCount)
		{
			_spinRoot = spinRoot;
			_spinDuration = spinDuration;
			_rotationAngle = rotationAngle;
			_numberOfSpins = numberOfSpins;
			_sliceCount = sliceCount;
		}

		public void SpinToIndex(int selectedIndex, TweenCallback onComplete)
		{
			var targetAngle = selectedIndex * (360f / _sliceCount);
			var totalRotation = _rotationAngle * _numberOfSpins + targetAngle;

			_spinRoot.DORotate(new Vector3(0, 0, -totalRotation), _spinDuration, RotateMode.FastBeyond360)
				.SetEase(Ease.OutCubic)
				.OnComplete(onComplete);
		}
	}
}