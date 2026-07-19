using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace LuckyPills; 

internal class LuckyPillsEventHandlers : CustomEventsHandler {
	public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev) {
		if (ev.UsableItem.Type != ItemType.Painkillers) {
			return;
		}
		// Prevent anythign from happening due to the painkillers.
		ev.IsAllowed = false;

		// Remove the painkillers as they are still used up by all intends and purposes.
		ev.Player.RemoveItem(ev.UsableItem);

		PillEffect.RunRandom(ev.Player);
	}
}
