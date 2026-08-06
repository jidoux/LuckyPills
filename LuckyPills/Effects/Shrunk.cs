namespace LuckyPills.Effects;

internal sealed class Shrunk : ShrunkConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been shrunk for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.Scale = new Vector3(0.2f, 0.2f, 0.2f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal class ShrunkConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 8f;
	public float MaxDuration { get; set; } = 16f;
	public float RarityMultiplier { get; set; } = 1f;
}
