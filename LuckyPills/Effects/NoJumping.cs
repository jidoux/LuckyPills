namespace LuckyPills.Effects;

internal sealed class NoJumping(NoJumpingConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You can no longer jump for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.HeavyFooted>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class NoJumpingConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 100f;
	public float MaxDuration { get; set; } = 200f;
	public float RarityMultiplier { get; set; } = 1f;
}
