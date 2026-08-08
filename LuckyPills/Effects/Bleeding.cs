namespace LuckyPills.Effects;

internal sealed class Bleeding : BleedingConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given bleeding for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Bleeding>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}


internal class BleedingConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 17f;
	public float MaxDuration { get; set; } = 45f;
	public float RarityMultiplier { get; set; } = 1f;
}
