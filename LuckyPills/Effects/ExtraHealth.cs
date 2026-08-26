namespace LuckyPills.Effects;

internal sealed class ExtraHealth(ExtraHealthConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => player.Health < 99f && config.IsEnabled;
	public string DisplayText { get; } = "You've been permanently given extra health";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.MaxHealth += config.AmountOfHealthToGive;
		player.Heal(player.MaxHealth);
	}
}

internal sealed class ExtraHealthConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float AmountOfHealthToGive { get; set; } = 100f;
}
