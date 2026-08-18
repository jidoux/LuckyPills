namespace LuckyPills.Effects;

internal sealed class GiveO5Keycard : GiveO5KeycardConfig, IPillEffect {
	public new bool IsEnabled(Player player) => !player.Items.Any(x => x.Base.name.Contains("keycard", StringComparison.OrdinalIgnoreCase)) && base.IsEnabled;
	public string DisplayText => "You've been given an 05 keycard";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.ForceEquip(ItemType.KeycardO5);
	}
}

internal class GiveO5KeycardConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
