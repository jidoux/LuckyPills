namespace LuckyPills.Effects;

internal sealed class Invigorated(InvigoratedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been invigorated for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Invigorated>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class InvigoratedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 120f;
	public float MaxDuration { get; set; } = 300f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
