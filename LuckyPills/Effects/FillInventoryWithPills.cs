namespace LuckyPills.Effects;

internal sealed class FillInventoryWithPills : FillInventoryWithPillsConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "Your inventory has been topped off with more pills";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		for (int i = 0; i < 8; i++) {
			player.AddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
	}
}

internal class FillInventoryWithPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
