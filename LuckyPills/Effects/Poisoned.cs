namespace LuckyPills.Effects;

internal sealed class Poisoned : PoisonedConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've poisoned yourself for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Poisoned>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class PoisonedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 1f;
}
