using VertigoGamesCaseStudy.Runtime.ScriptableObjects;
using VertigoGamesCaseStudy.Runtime.UI;
using VertigoGamesCaseStudy.Runtime.UI.Roulette;

namespace VertigoGamesCaseStudy.Core.Events
{
	public abstract class GameEvent
	{
		public int Priority { get; set; }

		protected GameEvent(int priority = 0)
		{
			Priority = priority;
		}
	}

	public class StartNewZoneEvent : GameEvent
	{
		public int ZoneNumber { get; private set; }
		public ZoneBase Zone { get; private set; }
		public StartNewZoneEvent(int zoneNumber, ZoneBase zone, int priority = 0)
			: base(priority)
		{
			ZoneNumber = zoneNumber;
			Zone = zone;
		}
	}

	public class RewardPopupExitEvent : GameEvent
	{
		public SlotItem SlotItem { get; private set; }

		public RewardPopupExitEvent(SlotItem slotItem, int priority = 0)
			: base(priority)
		{
			SlotItem = slotItem;
		}
	}

	public class RouletteSpinStartEvent : GameEvent
	{
		public RouletteSpinStartEvent(int priority = 0)
			: base(priority)
		{
		}
	}
	public class RouletteSpinEndEvent : GameEvent
	{
		public SlotItem SlotItem { get; private set; }
		public float RewardMultiplier { get; private set; }

		public RouletteSpinEndEvent(SlotItem slotItem, float rewardMultiplier, int priority = 0)
			: base(priority)
		{
			SlotItem = slotItem;
			RewardMultiplier = rewardMultiplier;
		}
	}

	public class BombExplodedEvent : GameEvent
	{
		public BombExplodedEvent(int priority = 0)
			: base(priority)
		{
		}
	}

	public class GiveUpEvent : GameEvent
	{
		public GiveUpEvent(int priority = 0)
			: base(priority)
		{
		}
	}

	public class ReviveEvent : GameEvent
	{
		public ReviveEvent(int priority = 0)
			: base(priority)
		{
		}
	}
	
	public class LeaveGameEvent : GameEvent
	{
		public LeaveGameEvent(int priority = 0)
			: base(priority)
		{
		}
	}
}