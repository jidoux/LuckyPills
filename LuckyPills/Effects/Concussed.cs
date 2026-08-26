namespace LuckyPills.Effects;

internal sealed class Concussed(ConcussedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been concussed for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Concussed>(intensity: byte.MaxValue, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Blurred>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class ConcussedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 30f; // TODO experiment with this one
	public float RarityMultiplier { get; set; } = 1f;
}
