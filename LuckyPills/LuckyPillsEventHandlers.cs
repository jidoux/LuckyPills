using InventorySystem;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp049Events;
using LabApi.Events.CustomHandlers;
using LuckyPills.Effects;

namespace LuckyPills;

internal sealed class LuckyPillsEventHandlers : CustomEventsHandler {
	private static readonly IReadOnlyCollection<IPillEffect> _allEffects = SharedCode.GetAllPillEffects().ToList();

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
			foreach (IPillEffect effect in _allEffects) {
				effect.OnDisabled(ev.Player);
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerEscaping(PlayerEscapingEventArgs ev) {
		try {
			// Need to disable any possible active effects here as well.
			foreach (IPillEffect effect in _allEffects) {
				effect.OnDisabled(ev.Player);
			}
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

	// I figure this is safe to avoid any manipulation with the handcuffed effect.
	public override void OnPlayerUncuffed(PlayerUncuffedEventArgs ev) {
		try {
			_allEffects.FirstOrDefault(x => x is Handcuffed && x.IsEnabled(ev.Player))?.OnDisabled(ev.Target);
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnScp049ResurrectedBody(Scp049ResurrectedBodyEventArgs ev) {
		try {
			if (Plugin.Singleton.Config.SpawnMinionsWithPills) {
				for (int i = 0; i < 8; i++) {
					ev.Target.Inventory.ServerAddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.AdminCommand);
				}
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}
}
