namespace LuckyPills.Effects;

internal sealed class Invisible : InvisibleConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been turned invisible for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None; // Not having this in good effects since it was more fun when the good effect player was visible.

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Fade>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class InvisibleConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 19f;
	public float MaxDuration { get; set; } = 35f;
	public float RarityMultiplier { get; set; } = 0.9f;
}
