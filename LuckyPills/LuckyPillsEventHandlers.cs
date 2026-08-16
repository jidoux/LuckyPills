using InventorySystem;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LuckyPills.Effects;

namespace LuckyPills;

internal sealed class LuckyPillsEventHandlers : CustomEventsHandler {
	private static readonly IReadOnlyCollection<IPillEffect> _allEffects = [.. SharedCode.GetAllPillEffects()];

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
			Debug.LogException(ex);
		}
	}

	public override void OnPlayerDying(PlayerDyingEventArgs ev) {
		try {
			// There were situations where players would die as a different size and respawn at that size.
			foreach (IPillEffect effect in _allEffects) {
				effect.OnDisabled(ev.Player);
			}
			ResurrectTeamMember.PlayerDied(ev.Player);
		}
		catch (Exception ex) {
			Debug.LogException(ex);
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
			Debug.LogException(ex);
		}
	}

	public override void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev) {
		try {
			if (EveryPickupTurnsIntoPainkillers.ShouldPickupTurnIntoPills(ev.Player)) {
				ev.IsAllowed = false;
				ev.Pickup.Destroy();
				ev.Player.Inventory.ServerAddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.PickedUp);
			}
		}
		catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	// I figure this is safe to avoid any manipulation with the handcuffed effect.
	public override void OnPlayerUncuffed(PlayerUncuffedEventArgs ev) {
		try {
			_allEffects.FirstOrDefault(x => x is Handcuffed && x.IsEnabled(ev.Player))?.OnDisabled(ev.Target);
		}
		catch (Exception ex) {
			Debug.LogException(ex);
		}
	}

	public override void OnServerRoundEnding(RoundEndingEventArgs ev) {
		try {
			// Not sure if ALL of these are even needed... there was an issue where I got the "every item you pickup
			// will turn into painkillers" and then the round ended and a different player got it, and it didnt work
			// for that player. Not sure how that would be possible, but I'm giving this a try and hoping it doesn't
			// happen again.
			foreach (IPillEffect effect in _allEffects) {
				effect.OnRoundEnded();
			}
		}
		catch (Exception ex) {
			Debug.LogException(ex);
		}
	}
}
