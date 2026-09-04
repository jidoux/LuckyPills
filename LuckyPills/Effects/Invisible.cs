namespace LuckyPills.Effects;

internal sealed class Invisible(InvisibleConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been turned invisible for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None; // Not having this in good effects since it was more fun when the good effect player was visible.

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Fade>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class InvisibleConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 20;
	public int MaxDuration { get; set; } = 40;
	public ushort RarityWeight { get; set; } = 90;
}
