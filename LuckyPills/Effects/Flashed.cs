namespace LuckyPills.Effects;

internal sealed class Flashed(FlashedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been flashed";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Flashed>(intensity: 5, duration: duration, addDuration: true);
	}
}

internal sealed class FlashedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 5;
	public int MaxDuration { get; set; } = 5;
	public ushort RarityWeight { get; set; } = 50;
}
