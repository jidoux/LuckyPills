using InventorySystem;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.ThrowableProjectiles;
using Mirror;
using UnityEngine;
using ThrowableItem = InventorySystem.Items.ThrowableProjectiles.ThrowableItem;

internal static class Grenades {
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
}
