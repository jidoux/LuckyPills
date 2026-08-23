namespace LuckyPills.Effects;

internal sealed class GiveO5Keycard : GiveO5KeycardConfig, IPillEffect {
	public new bool IsEnabled(Player player) {
		if (!base.IsEnabled) {
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
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.ForceEquip(ItemType.KeycardO5);
	}
}

internal class GiveO5KeycardConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
