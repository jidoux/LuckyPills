namespace LuckyPills.Effects;

internal sealed class Ensnared(EnsnaredConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been ensnared for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Ensnared>(intensity: 5, duration: duration, addDuration: true);
	}
}

internal sealed class EnsnaredConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 5f;
	public float MaxDuration { get; set; } = 10f;
	public float RarityMultiplier { get; set; } = 1f;
}
