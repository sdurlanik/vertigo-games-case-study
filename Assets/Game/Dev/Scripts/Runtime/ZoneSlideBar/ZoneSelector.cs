using VertigoGamesCaseStudy.Runtime.Interfaces;
using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.UI.ZoneSlide
{
	public class ZoneSelector : IZoneSelector
	{
		private readonly SuperZoneSO _superZoneSo;
		private readonly SafeZoneSO _safeZoneSo;
		private readonly DefaultZoneSO _defaultZoneSo;

		public ZoneSelector(SuperZoneSO superZoneSo, SafeZoneSO safeZoneSo, DefaultZoneSO defaultZoneSo)
		{
			_superZoneSo = superZoneSo;
			_safeZoneSo = safeZoneSo;
			_defaultZoneSo = defaultZoneSo;
		}

		public ZoneBase GetCurrentZone(int currentZoneNumber)
		{
			if (currentZoneNumber % _superZoneSo.triggerInterval == 0)
			{
				return _superZoneSo;
			}

			if (currentZoneNumber % _safeZoneSo.triggerInterval == 0)
			{
				return _safeZoneSo;
			}

			return _defaultZoneSo;
		}
	}
}