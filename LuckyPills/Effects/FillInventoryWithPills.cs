using InventorySystem;

namespace LuckyPills.Effects;

internal sealed class FillInventoryWithPills : FillInventoryWithPillsConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "Your inventory has been filled with more pills";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		for (int i = 0; i < 8; i++) {
			player.Inventory.ServerAddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
	}
}

internal class FillInventoryWithPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
