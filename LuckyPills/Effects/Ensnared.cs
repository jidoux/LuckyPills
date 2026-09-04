namespace LuckyPills.Effects;

internal sealed class Ensnared(EnsnaredConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been ensnared for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Ensnared>(intensity: 5, duration: duration, addDuration: true);
	}
}

internal sealed class EnsnaredConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 5;
	public int MaxDuration { get; set; } = 10;
	public ushort RarityWeight { get; set; } = 100;
}
