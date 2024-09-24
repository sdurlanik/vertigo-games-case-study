using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VertigoGamesCaseStudy.Runtime.Interfaces
{
    public interface IZoneColorProvider
    {
        Color32 GetZoneNumberColor(int zoneNumber, bool isCurrentZone = false);
    }
}
