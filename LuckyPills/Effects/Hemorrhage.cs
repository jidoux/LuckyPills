namespace LuckyPills.Effects;

internal sealed class Hemorrhage(HemorrhageConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've begun to hemorrhage for the next {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Hemorrhage>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class HemorrhageConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 70f; // TODO idek if this effect works lol :skull:
	public float RarityMultiplier { get; set; } = 1f;
}
