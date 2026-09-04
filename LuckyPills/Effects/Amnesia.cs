namespace LuckyPills.Effects;

internal sealed class Amnesia(AmnesiaConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given amnesia for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.AmnesiaVision>(intensity: byte.MaxValue, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Blurred>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class AmnesiaConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 37;
	public ushort RarityWeight { get; set; } = 100;
}
