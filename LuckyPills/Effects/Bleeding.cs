namespace LuckyPills.Effects;

internal sealed class Bleeding(BleedingConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given bleeding for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Bleeding>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}


internal sealed class BleedingConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 17f;
	public float MaxDuration { get; set; } = 45f;
	public float RarityMultiplier { get; set; } = 1f;
}
