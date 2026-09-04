namespace LuckyPills.Effects;

internal sealed class Bleeding(BleedingConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given bleeding for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Bleeding>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}


internal sealed class BleedingConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 17;
	public int MaxDuration { get; set; } = 45;
	public ushort RarityWeight { get; set; } = 100;
}
