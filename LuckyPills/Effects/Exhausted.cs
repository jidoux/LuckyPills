namespace LuckyPills.Effects;

internal sealed class Exhausted(ExhaustedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've become exhausted for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Exhausted>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class ExhaustedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 60;
	public ushort RarityWeight { get; set; } = 46; // I mean seriously this is one of the lamest ones in my opinion
}
