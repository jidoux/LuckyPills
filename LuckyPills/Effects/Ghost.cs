namespace LuckyPills.Effects;

internal sealed class Ghost : GhostConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "You've become a ghost for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent; // TODO making this goodaspermanent maybe crazy broken idk

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Ghostly>(intensity: 1, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.SilentWalk>(intensity: 1, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Fade>(intensity: 240, duration: duration, addDuration: true);
	}
}

internal class GhostConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 30f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 1f;
}
