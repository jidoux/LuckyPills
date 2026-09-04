namespace LuckyPills.Effects;

internal sealed class Hemorrhage(HemorrhageConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've begun to hemorrhage for the next {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Hemorrhage>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class HemorrhageConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 20;
	public int MaxDuration { get; set; } = 70; // TODO idek if this effect works lol :skull:
	public ushort RarityWeight { get; set; } = 100;
}
