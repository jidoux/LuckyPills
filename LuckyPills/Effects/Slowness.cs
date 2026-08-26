namespace LuckyPills.Effects;

internal sealed class Slowness(SlownessConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given slowness for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Slowness>(intensity: 30, duration: duration, addDuration: true);
	}
}

internal sealed class SlownessConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 35f;
	public float RarityMultiplier { get; set; } = 0.6f;
}
