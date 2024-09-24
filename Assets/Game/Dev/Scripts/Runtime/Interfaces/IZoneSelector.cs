using VertigoGamesCaseStudy.Runtime.ScriptableObjects;

namespace VertigoGamesCaseStudy.Runtime.Interfaces
{
	public interface IZoneSelector
	{
		ZoneBase GetCurrentZone(int currentZoneNumber);
	}
}