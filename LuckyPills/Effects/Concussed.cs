namespace LuckyPills.Effects;

internal sealed class Concussed(ConcussedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been concussed for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Concussed>(intensity: byte.MaxValue, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Blurred>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class ConcussedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 30; // TODO experiment with this one
	public ushort RarityWeight { get; set; } = 100;
}
