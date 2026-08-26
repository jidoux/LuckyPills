namespace LuckyPills.Effects;

internal sealed class Poisoned(PoisonedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've poisoned yourself for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Poisoned>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class PoisonedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 50f; // TODO experiment with this one
	public float RarityMultiplier { get; set; } = 1f;
}
