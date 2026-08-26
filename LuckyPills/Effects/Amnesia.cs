namespace LuckyPills.Effects;

internal sealed class Amnesia(AmnesiaConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given amnesia for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.AmnesiaVision>(intensity: byte.MaxValue, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Blurred>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class AmnesiaConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 37f;
	public float RarityMultiplier { get; set; } = 1f;
}
