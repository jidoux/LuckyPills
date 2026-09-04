namespace LuckyPills.Effects;

internal sealed class Blinded(BlindedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been blinded for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Blindness>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class BlindedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 20;
	public ushort RarityWeight { get; set; } = 50;
}
