namespace LuckyPills.Effects;

internal sealed class Invisible(InvisibleConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been turned invisible for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None; // Not having this in good effects since it was more fun when the good effect player was visible.

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Fade>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class InvisibleConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 40f;
	public float RarityMultiplier { get; set; } = 0.9f;
}
