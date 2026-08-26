namespace LuckyPills.Effects;

internal sealed class SuperSpeed(SuperSpeedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given super speed for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.MovementBoost>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class SuperSpeedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 45f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
