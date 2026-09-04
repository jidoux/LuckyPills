namespace LuckyPills.Effects;

internal sealed class NoJumping(NoJumpingConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You can no longer jump for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.HeavyFooted>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class NoJumpingConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 100;
	public int MaxDuration { get; set; } = 200;
	public ushort RarityWeight { get; set; } = 100;
}
