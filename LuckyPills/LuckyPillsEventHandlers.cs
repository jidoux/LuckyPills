using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace LuckyPills; 

internal class LuckyPillsEventHandlers : CustomEventsHandler {
	public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev) {
		try {
			if (ev.UsableItem.Type != ItemType.Painkillers) {
				return;
			}
			// Prevent anythign from happening due to the painkillers.
			ev.IsAllowed = false;

			// Remove the painkillers as they are still used up by all intends and purposes.
			ev.Player.RemoveItem(ev.UsableItem);

			PillEffectOrchestrator.RunRandom(ev.Player);
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}

	}

	public override void OnPlayerDeath(PlayerDeathEventArgs ev) {
		try {
			// During testing there were situations where player scale changed, and they wouldn't change back
			ev.Player.Scale = Vector3.one;
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}
}
