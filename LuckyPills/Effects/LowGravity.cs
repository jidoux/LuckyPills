namespace LuckyPills.Effects;

internal sealed class LowGravity : LowGravityConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given low gravity for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.Gravity = new Vector3(0f, -1f, -0f);
	}

	public void OnDisabled(Player player) {
		player.Gravity = new Vector3(0f, -19.6f, 0f);
	}
}

internal class LowGravityConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 20f;
	public float RarityMultiplier { get; set; } = 1f;
}
