namespace LuckyPills.Effects;

internal sealed class Ghost(GhostConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've become a ghost for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent; // TODO making this goodaspermanent maybe crazy broken idk

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Ghostly>(intensity: 1, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.SilentWalk>(intensity: 1, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Fade>(intensity: 240, duration: duration, addDuration: true);
	}
}

internal sealed class GhostConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 30f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
