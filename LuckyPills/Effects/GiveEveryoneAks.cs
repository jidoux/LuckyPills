using InventorySystem;
using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks : GiveEveryoneAksConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've given every player an AK";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player anyPlayerInMap in Player.List.Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			anyPlayerInMap.SendHint("You've been given an AK by someone else's Painkillers");
			// TODO validate that this is fine. If inventory is full, spawn it on ground. I'd prefer
			// if this was abstracted away in some ItemAddReason but for now don't have time to do that.
			if (anyPlayerInMap.Items.Count() >= 8) {
				Pickup.Create(ItemType.GunAK, anyPlayerInMap.Position);
			}
			else {
				anyPlayerInMap.Inventory.ServerAddItem(ItemType.GunAK, InventorySystem.Items.ItemAddReason.AdminCommand);
			}
		}
	}
}

internal class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.9f;
}
