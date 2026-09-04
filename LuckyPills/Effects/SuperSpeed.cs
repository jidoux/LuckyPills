namespace LuckyPills.Effects;

internal sealed class SuperSpeed(SuperSpeedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given super speed for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.MovementBoost>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class SuperSpeedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 45;
	public ushort RarityWeight { get; set; } = 95;
}
