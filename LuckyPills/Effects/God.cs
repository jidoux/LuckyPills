namespace LuckyPills.Effects;

internal sealed class God(GodConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given god mode for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.IsGodModeEnabled = true;
	}

	public void OnDisabled(Player player) {
		player.IsGodModeEnabled = false;
	}
}

internal sealed class GodConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 0.9f;
}
