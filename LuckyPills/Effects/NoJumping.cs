namespace LuckyPills.Effects;

internal sealed class NoJumping : NoJumpingConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You can no longer jump for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.HeavyFooted>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class NoJumpingConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 100f;
	public float MaxDuration { get; set; } = 200f;
	public float RarityMultiplier { get; set; } = 1f;
}
