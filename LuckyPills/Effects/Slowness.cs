namespace LuckyPills.Effects;

internal sealed class Slowness(SlownessConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given slowness for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Slowness>(intensity: 30, duration: duration, addDuration: true);
	}
}

internal sealed class SlownessConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 35;
	public ushort RarityWeight { get; set; } = 60;
}
