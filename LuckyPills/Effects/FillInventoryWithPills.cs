namespace LuckyPills.Effects;

internal sealed class FillInventoryWithPills(FillInventoryWithPillsConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "Your inventory has been topped off with more pills";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
		for (int i = 0; i < 8; i++) {
			player.AddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
	}
}

internal sealed class FillInventoryWithPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
