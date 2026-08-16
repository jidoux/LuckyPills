using InventorySystem;

namespace LuckyPills.Effects;

internal sealed class FillInventoryWithPills : FillInventoryWithPillsConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "Your inventory has been filled with more pills";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.DropAllItems(); // Lets just see how this goes, why not :) TODO validate that this is fine.
		for (int i = 0; i < 8; i++) {
			player.Inventory.ServerAddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
	}
}

internal class FillInventoryWithPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
