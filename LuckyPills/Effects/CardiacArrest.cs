namespace LuckyPills.Effects;

internal sealed class CardiacArrest(CardiacArrestConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given cardiac arrest for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.CardiacArrest>(intensity: 1, duration: duration, addDuration: true);
	}
}

internal sealed class CardiacArrestConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 10f;
	public float RarityMultiplier { get; set; } = 0.9f;
}
