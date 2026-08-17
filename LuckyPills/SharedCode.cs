using Hazards;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using RelativePositioning;
using System.Diagnostics.CodeAnalysis;
using ThrowableItem = InventorySystem.Items.ThrowableProjectiles.ThrowableItem;

namespace LuckyPills;

/// <summary>
/// Just a dump of any shared helper methods... I prefer this over separate files due to small scale.
/// </summary>
internal static class SharedCode {
	/// <summary>
	/// Generally should just pass in the player.position - will spawn an active Scp244 of random type.
	/// </summary>
	public static void SpawnScp244(Vector3 positionToSpawnIt, Quaternion? rotationInput = null, Vector3? scaleInput = null) {
		Quaternion rotation = rotationInput ?? Quaternion.identity;
		Vector3 scale = scaleInput ?? Vector3.one;

		ItemType itemType = ItemType.SCP244a;
		if (Random.Range(1, 3) == 1) { // not sure the cleanest way to write 50% chances but this is 50% chance
			itemType = ItemType.SCP244b;
		}

		Scp244Pickup? pickup = (Scp244Pickup?)Pickup.Create(type: itemType, position: positionToSpawnIt, rotation: rotation, scale: scale);
		if (pickup is not null) {
			pickup.State = InventorySystem.Items.Usables.Scp244.Scp244State.Active;
		}
	}

	public static IEnumerator<float> RunGrenadeVomit(Player player, float duration, float grenadesPerSecond, ItemType itemType) {
		float delayTime = 1f / grenadesPerSecond;
		for (int i = 0; i < duration * grenadesPerSecond; i++) {
			if (!player.IsAlive) {
				yield break;
			}

			SpawnThrownExplosive(player.ReferenceHub, itemType);
			yield return MEC.Timing.WaitForSeconds(delayTime);
		}
	}

	private static void SpawnThrownExplosive(ReferenceHub thrower, ItemType grenadeType, float forceMultiplier = 1f, float upwardMultiplier = 1f) {
		if (!InventoryItemLoader.TryGetItem(grenadeType, out ThrowableItem grenadeTemplate)) {
			Logger.Error($"Failed to get the following grenade type (for bomb vomit): {grenadeType}");
			return;
		}
		float forceAmount = grenadeTemplate.FullThrowSettings.StartVelocity * forceMultiplier;
		float upwardFactor = grenadeTemplate.FullThrowSettings.UpwardsFactor * upwardMultiplier;
		Vector3 startTorque = grenadeTemplate.FullThrowSettings.StartTorque;
		Vector3 startVel = ThrowableNetworkHandler.GetLimitedVelocity(new Vector3(0, 0, 0));

		ThrownProjectile thrownProjectile = UnityEngine.Object.Instantiate(grenadeTemplate.Projectile, thrower.PlayerCameraReference.position, thrower.PlayerCameraReference.rotation);
		PickupSyncInfo info = new() {
			ItemId = grenadeType,
			Locked = true,
		};
		thrownProjectile.Info = info;
		thrownProjectile.PreviousOwner = new Footprinting.Footprint(thrower);
		NetworkServer.Spawn(thrownProjectile.gameObject, ownerConnection: null);
		if (thrownProjectile.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
			float d = 1f - Mathf.Abs(Vector3.Dot(thrower.PlayerCameraReference.forward, Vector3.up));
			Vector3 forward = thrower.PlayerCameraReference.forward;
			Vector3 a = thrower.PlayerCameraReference.up * upwardFactor;
			Vector3 a2 = forward + a * d;
			rb.centerOfMass = Vector3.zero;
			rb.angularVelocity = startTorque;
			rb.linearVelocity = startVel + a2 * forceAmount;
		}
		thrownProjectile.ServerActivate();
	}

	public static IEnumerable<IPillEffect> GetAllPillEffects(bool useDebugPickPills = false)
		=> useDebugPickPills
			? typeof(IPillEffect).Assembly.GetTypes()
				.Where(x => typeof(IDebugPickPills).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(x => (IPillEffect)Activator.CreateInstance(x)!)
			: typeof(IPillEffect).Assembly.GetTypes()
				.Where(x => typeof(IPillEffect).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(x => (IPillEffect)Activator.CreateInstance(x)!);

	public static void EnablePillEffect(IPillEffect effect, Player player, float duration) {
		effect.OnEnabled(player, duration);
		MEC.Timing.CallDelayed(duration, () => effect.OnDisabled(player));
	}

	public static bool TryGetScp079TierManager(PlayerRoleBase playerRoleBase, [NotNullWhen(true)] out Scp079TierManager? scp079TierManager) {
		if (playerRoleBase is Scp079Role scp079Role && scp079Role.SubroutineModule.TryGetSubroutine<Scp079TierManager>(out Scp079TierManager? tierManager) && tierManager is not null) {
			scp079TierManager = tierManager;
			return true;
		}
		scp079TierManager = null;
		return false;
	}

	/// <summary>
	/// Sets SCP-079's exp and levels it accordingly, whereas just increasing the exp by 1000 or whatever will
	/// just make it level 2. May not work in the future if SCP-079 leveling thresholds change.
	/// </summary>
	public static void SetScp079ExpLevel(Scp079TierManager scp079TierManager, int expToSetTo) {
		if (expToSetTo > 80) {
			scp079TierManager.TotalExp += 80;
			expToSetTo -= 80;
		}
		if (expToSetTo > 130) {
			scp079TierManager.TotalExp += 130;
			expToSetTo -= 130;
		}
		if (expToSetTo > 250) {
			scp079TierManager.TotalExp += 250;
			expToSetTo -= 250;
		}
		if (expToSetTo > 500) {
			scp079TierManager.TotalExp += 500;
			expToSetTo -= 500;
		}
		scp079TierManager.TotalExp += expToSetTo;
	}

	public static T? GetPrefab<T>() {
		foreach (GameObject prefab in NetworkClient.prefabs.Values) {
			if (!prefab.TryGetComponent(out T prefabToReturn)) {
				continue;
			}

			return prefabToReturn;
		}

		return default;
	}

	public static void SpawnTantrum(Vector3 position) {
		TantrumEnvironmentalHazard? tantrum = UnityEngine.Object.Instantiate(SharedCode.GetPrefab<TantrumEnvironmentalHazard>());
		if (tantrum is null) {
			Logger.Error("Failed to instantiate Tantrum hazard for Tantrum pill effect... something changed?? Idk. Cancelling...");
			return;
		}
		tantrum.SynchronizedPosition = new RelativePosition(position);

		NetworkServer.Spawn(tantrum.gameObject);
	}

	public static void SpawnItemBelow(this Player? player, ItemType itemType, ItemAddReason itemAddReason = ItemAddReason.AdminCommand) {
		if (player is null) {
			return;
		}
		ItemBase item = player.Inventory.ServerAddItem(itemType, itemAddReason);
		player.DropItem(item);
	}

	public static void ForceEquip(this Player? player, ItemType itemType, ItemAddReason itemAddReason = ItemAddReason.AdminCommand) {
		if (player is null) {
			return;
		}
		Item? item = player.AddItem(itemType, itemAddReason);
		if (item is not null) {
			player.CurrentItem = item;
		}
	}
}
