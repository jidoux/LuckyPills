namespace LuckyPills.Effects;

internal sealed class Amnesia : AmnesiaConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given amnesia for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.AmnesiaVision>(intensity: byte.MaxValue, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Blurred>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class AmnesiaConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 11f;
	public float MaxDuration { get; set; } = 27f;
	public float RarityMultiplier { get; set; } = 1f;
}
