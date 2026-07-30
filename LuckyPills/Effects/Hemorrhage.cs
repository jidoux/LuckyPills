namespace LuckyPills.Effects;

internal sealed class Hemorrhage : HemorrhageConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've begun to hemorrhage for the next {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Hemorrhage>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class HemorrhageConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 8f;
	public float MaxDuration { get; set; } = 15f;
	public float RarityMultiplier { get; set; } = 1f;
}
