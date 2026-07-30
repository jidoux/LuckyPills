using InventorySystem;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using Mirror;
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
}
