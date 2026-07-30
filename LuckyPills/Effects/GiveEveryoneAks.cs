using InventorySystem;
using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks: GiveEveryoneAksConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've given every player an AK";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player anyPlayerInMap in Player.List.Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			anyPlayerInMap.Inventory.ServerAddItem(ItemType.GunAK, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
	}
}

internal class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
