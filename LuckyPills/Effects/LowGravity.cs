namespace LuckyPills.Effects;

internal sealed class LowGravity(LowGravityConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given low gravity for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.Gravity = new Vector3(0f, -1f, -0f);
	}

	public void OnDisabled(Player player) {
		player.Gravity = new Vector3(0f, -19.6f, 0f);
	}
}

internal sealed class LowGravityConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 40f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
