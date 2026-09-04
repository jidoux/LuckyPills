namespace LuckyPills.Effects;

internal sealed class Poisoned(PoisonedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've poisoned yourself for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Poisoned>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class PoisonedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 20;
	public int MaxDuration { get; set; } = 50; // TODO experiment with this one
	public ushort RarityWeight { get; set; } = 100;
}
