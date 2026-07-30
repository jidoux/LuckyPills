using InventorySystem;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;

namespace LuckyPills; 

internal sealed class LuckyPillsEventHandlers : CustomEventsHandler {
	private static readonly List<IPillEffect> _allEffects = SharedCode.GetAllPillEffects().ToList();

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

	public override void OnPlayerDying(PlayerDyingEventArgs ev) {
		try {
			// There were situations where players would die as a different size and respawn at that size.
			_allEffects.ForEach(x => x.OnDisabled(ev.Player));
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerEscaping(PlayerEscapingEventArgs ev) {
		try {
			// Need to disable any possible active effects here as well.
			_allEffects.ForEach(x => x.OnDisabled(ev.Player));
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev) {
		try {
			if (GlobalVariables.PlayersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Contains(ev.Player)) {
				ev.IsAllowed = false;
				ev.Pickup.Destroy();
				ev.Player.Inventory.ServerAddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.PickedUp);
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}
}
