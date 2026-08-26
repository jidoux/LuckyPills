namespace LuckyPills.Effects;

internal sealed class Exhausted(ExhaustedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've become exhausted for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Exhausted>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class ExhaustedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 0.46f; // I mean seriously this is one of the lamest ones in my opinion
}
