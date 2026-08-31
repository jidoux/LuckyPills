using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks(GiveEveryoneAksConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've given every player an AK";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player anyPlayerInMap in Player.ReadyList.Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			if (anyPlayerInMap.Items.Count() >= 8) {
				//SpawnFirearmBelow(player.Position, ItemType.GunAK);
			}
			else {
				if (anyPlayerInMap != player) {
					anyPlayerInMap.SendHint("You've been given an AK by someone else's Painkillers", duration: 4);
				}
				anyPlayerInMap.ForceEquip(ItemType.GunAK);
			}
		}
	}

	// TODO - in the future, fix this to handle full inventories. Unsure how to do it yet... spawning the gun
	// is ez but how to make it spawn with ammo idk lol
	//private static void SpawnFirearmBelow(Vector3 position, ItemType itemType) {
	//	FirearmPickup? item = (FirearmPickup)FirearmPickup.Create(itemType, position);
	//	item.Base.
	//}
}

internal sealed class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.8f;
}
