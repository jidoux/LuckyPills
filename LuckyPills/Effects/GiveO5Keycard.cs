namespace LuckyPills.Effects;

internal sealed class GiveO5Keycard(GiveO5KeycardConfig config) : IPillEffect {
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		foreach (Item item in player.Items) {
			if (item.Type == ItemType.KeycardScientist || item.Type == ItemType.KeycardZoneManager || item.Type == ItemType.KeycardJanitor) {
				continue;
			}
			// If they have any keycard which is not a scientist, zone manager, or janitor, then don't enable this effect
			if (item.Base.name.Contains("keycard", StringComparison.OrdinalIgnoreCase)) {
				return false;
			}
		}
		return true;
	}
	public string DisplayText { get; } = "You've been given an 05 keycard";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
		player.ForceEquip(ItemType.KeycardO5);
	}
}

internal sealed class GiveO5KeycardConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
