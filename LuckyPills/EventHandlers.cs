using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.Scp0492Events;
using LabApi.Events.Arguments.Scp096Events;
using LabApi.Events.Arguments.Scp127Events;
using LabApi.Events.Arguments.Scp173Events;
using LabApi.Events.Arguments.Scp914Events;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LuckyPills.Effects;

namespace LuckyPills;

internal sealed class EventHandlers : CustomEventsHandler {
	public override void OnPlayerUsingItem(PlayerUsingItemEventArgs ev) {
		LogCallIfDebug();
		try {
			if (ev.UsableItem.Type != ItemType.Painkillers) {
				return;
			}
			ev.Player.RemoveItem(ev.UsableItem); // Need to remove the item now since some effects can add an item.
			if (!NextRoundEveryPillIsBallVomit.TryDoNextRoundBallVomitBehavior(ev.Player)) {
				RunRandom(ev.Player);
			}
			ev.IsAllowed = false;
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerDying(PlayerDyingEventArgs ev) {
		LogCallIfDebug();
		try {
			// There were situations where players would die as a different size and respawn at that size.
			foreach (IPillEffect effect in AllPillEffects) {
				effect.OnDisabled(ev.Player);
			}
			ResurrectTeamMember.PlayerDied(ev.Player);
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerEscaping(PlayerEscapingEventArgs ev) {
		LogCallIfDebug();
		try {
			// This gets triggered no matter what role you are.
			if (ev.Player.Role == PlayerRoles.RoleTypeId.ClassD || ev.Player.Role == PlayerRoles.RoleTypeId.Scientist) {
				// Need to disable any possible active effects here as well.
				foreach (IPillEffect effect in AllPillEffects) {
					effect.OnDisabled(ev.Player);
				}
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerPickingUpItem(PlayerPickingUpItemEventArgs ev) {
		LogCallIfDebug();
		try {
			if (EveryPickupTurnsIntoPainkillers.ShouldPickupTurnIntoPills(ev.Player)) {
				ev.IsAllowed = false;
				ev.Pickup.Destroy();
				ev.Player.AddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.PickedUp);
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	// I figure this is safe to avoid any manipulation with the handcuffed effect.
	public override void OnPlayerUncuffed(PlayerUncuffedEventArgs ev) {
		LogCallIfDebug();
		try {
			new Handcuffed(Plugin.Singleton.Config.Handcuffed).OnDisabled(ev.Target);
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnServerRoundEnding(RoundEndingEventArgs ev) {
		LogCallIfDebug();
		try {
			// Not sure if ALL of these are even needed... there was an issue where I got the "every item you pickup
			// will turn into painkillers" and then the round ended and a different player got it, and it didnt work
			// for that player. Not sure how that would be possible, but I'm giving this a try and hoping it doesn't
			// happen again.
			foreach (IPillEffect effect in AllPillEffects) {
				effect.OnRoundEnd();
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	public override void OnPlayerSpawned(PlayerSpawnedEventArgs ev) {
		LogCallIfDebug();
		// TODO this did not work for minions, unsure why.
		Timing.CallDelayed(Timing.WaitForOneFrame, () => {
			NextRoundLogicers.NextRoundLogicersBehavior(ev.Player);
			NextRoundFastRound.NextRoundFastRoundBehavior(ev.Player);
			NextRoundTinyRound.NextRoundTinyRoundBehavior(ev.Player);
			NextRoundTrippyRound.NextRoundTrippyRoundBehavior(ev.Player);
		});
	}

	public override void OnServerItemSpawning(ItemSpawningEventArgs ev) {
		LogCallIfDebug();
		if (NextRoundNoPills.ShouldNotSpawnPills(ev.ItemType)) {
			ev.IsAllowed = false;
		}
	}

	public override void OnServerPickupCreated(PickupCreatedEventArgs ev) {
		LogCallIfDebug();
		if (NextRoundNoPills.ShouldNotSpawnPills(ev.Pickup.Type)) {
			ev.Pickup.Destroy();
		}
	}

	public override void OnServerRoundStarted() {
		LogCallIfDebug();
		// I'm not 100% sure, but I think this does need to be delayed by a frame.
		Timing.CallDelayed(Timing.WaitForOneFrame, () => {
			NextRoundSurfaceFight.NextRoundSurfaceFightBehavior();
			NextRoundInstaDeadman.NextRoundInstaDeadmanBehavior();
		});
	}

	public override void OnPlayerRoomChanged(PlayerRoomChangedEventArgs ev) {
		LogCallIfDebug();
		RoomWhichKillsYou.PlayerEnteredRoom(ev.Player, ev.NewRoom);
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerActivatedGenerator(PlayerActivatedGeneratorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedGenerator(PlayerInteractedGeneratorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInspectedItem(PlayerInspectedItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerCancelledUsingItem(PlayerCancelledUsingItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedScp330(PlayerInteractedScp330EventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerChangedAttachments(PlayerChangedAttachmentsEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerAimedWeapon(PlayerAimedWeaponEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDryFiredWeapon(PlayerDryFiredWeaponEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerChangedBadgeVisibility(PlayerChangedBadgeVisibilityEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerUsedRadio(PlayerUsedRadioEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerCuffed(PlayerCuffedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Target); // Target is the player who was detained.
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player); // Player is the player who is detaining another one.
	}
	public override void OnPlayerEnteredHazard(PlayerEnteredHazardEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDetectedByScp1344(PlayerDetectedByScp1344EventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Target); // Target is the player who has been detected
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player); // Player is the player who has SCP-1344
	}
	public override void OnPlayerProcessedJailbirdMessage(PlayerProcessedJailbirdMessageEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerProcessedScp1509Message(PlayerProcessedScp1509MessageEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	// Okay I just couldn't resist; this is just too funny lol
	public override void OnPlayerReportedPlayer(PlayerReportedPlayerEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player); // Player is the player who sent reported
	}
	public override void OnPlayerUnloadedWeapon(PlayerUnloadedWeaponEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerEnteredPocketDimension(PlayerEnteredPocketDimensionEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerChangedItem(PlayerChangedItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerChangedRadioRange(PlayerChangedRadioRangeEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerClosedGenerator(PlayerClosedGeneratorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDroppedAmmo(PlayerDroppedAmmoEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerOpenedGenerator(PlayerOpenedGeneratorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedDoor(PlayerInteractedDoorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerTriggeredTesla(PlayerTriggeredTeslaEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerUnlockedWarheadButton(PlayerUnlockedWarheadButtonEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerUsedIntercom(PlayerUsedIntercomEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerShotWeapon(PlayerShotWeaponEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDamagedWindow(PlayerDamagedWindowEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerUsedItem(PlayerUsedItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerReceivedAchievement(PlayerReceivedAchievementEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerIdledTesla(PlayerIdledTeslaEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerPickedUpScp330(PlayerPickedUpScp330EventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerPickedUpAmmo(PlayerPickedUpAmmoEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerPickedUpArmor(PlayerPickedUpArmorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerPickedUpItem(PlayerPickedUpItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedElevator(PlayerInteractedElevatorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedLocker(PlayerInteractedLockerEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedToy(PlayerInteractedToyEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInteractedWarheadLever(PlayerInteractedWarheadLeverEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerToggledWeaponFlashlight(PlayerToggledWeaponFlashlightEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerToggledRadio(PlayerToggledRadioEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerToggledFlashlight(PlayerToggledFlashlightEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerToggledDisruptorFiringMode(PlayerToggledDisruptorFiringModeEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerLeftHazard(PlayerLeftHazardEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerScp1509Resurrected(PlayerScp1509ResurrectedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerSpinnedRevolver(PlayerSpinnedRevolverEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnScp914ProcessedPlayer(Scp914ProcessedPlayerEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnScp914KnobChanged(Scp914KnobChangedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	// I figure it would be so funny that it happens that I'm fine keeping the next few in even though its likely never gonna happen lol
	// There is a pill which can give SCPs pills (if its enabled, plus admins can just spawn them in, so I think its fine, wahtevs.
	public override void OnScp0492StartedConsumingCorpse(Scp0492StartedConsumingCorpseEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnScp096TryingNotToCry(Scp096TryingNotToCryEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnScp096PriedGate(Scp096PriedGateEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnScp173CreatedTantrum(Scp173CreatedTantrumEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnScp127LevelUp(Scp127LevelUpEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Scp127Item.CurrentOwner);
	}
	public override void OnScp914Activated(Scp914ActivatedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	//public override void OnPlayerMovementStateChanged(PlayerMovementStateChangedEventArgs ev) {
	//	FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	//}
	public override void OnPlayerEscaped(PlayerEscapedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerFlippedCoin(PlayerFlippedCoinEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDroppedItem(PlayerDroppedItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerChangedRole(PlayerChangedRoleEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerJumped(PlayerJumpedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerInspectedKeycard(PlayerInspectedKeycardEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerThrewItem(PlayerThrewItemEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerThrewProjectile(PlayerThrewProjectileEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDamagedShootingTarget(PlayerDamagedShootingTargetEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerHurt(PlayerHurtEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerDeactivatedGenerator(PlayerDeactivatedGeneratorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerZoneChanged(PlayerZoneChangedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerUnlockedGenerator(PlayerUnlockedGeneratorEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerReloadedWeapon(PlayerReloadedWeaponEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerSendingVoiceMessage(PlayerSendingVoiceMessageEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
	public override void OnPlayerUpdatedEffect(PlayerEffectUpdatedEventArgs ev) {
		FutureDeathRisk.FutureDeathRiskBehavior(ev.Player);
	}
}
