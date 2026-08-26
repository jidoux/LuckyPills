namespace LuckyPills.Effects;

internal sealed class Flashed(FlashedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been flashed";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Flashed>(intensity: 5, duration: duration, addDuration: true);
	}
}

internal sealed class FlashedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 5f;
	public float MaxDuration { get; set; } = 5f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
