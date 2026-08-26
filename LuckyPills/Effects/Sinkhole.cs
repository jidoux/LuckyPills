namespace LuckyPills.Effects;

internal sealed class Sinkhole(SinkholeConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given a sinkhole effect for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Sinkhole>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class SinkholeConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
