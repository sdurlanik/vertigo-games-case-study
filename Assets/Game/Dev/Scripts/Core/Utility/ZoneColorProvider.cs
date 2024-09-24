using UnityEngine;
using VertigoGamesCaseStudy.Runtime.Interfaces;

namespace VertigoGamesCaseStudy.Core.Utility
{
    public class ZoneColorProvider : IZoneColorProvider
    {
        public Color32 GetZoneNumberColor(int zoneNumber, bool isCurrentZone = false)
        {
            if (zoneNumber % 30 == 0)
            {
                return new Color32(255, 165, 0, 255); // Orange
            }

            if (zoneNumber % 5 == 0)
            {
                return new Color32(96, 255, 4, 255); // Green
            }

            if (isCurrentZone)
            {
                return new Color32(0, 0, 0, 255); // Black
            }

            return new Color32(255, 255, 255, 255); // White
        }
    }
}
