namespace LuckyPills.Effects;

internal sealed class LightBulbVomit(LightBulbVomitConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given light bulb vomit for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.VomitEffect;

	public void OnEnabled(Player player, float duration) {
		MEC.Timing.RunCoroutine(RunGrenadeVomit(player, duration, config.GrenadesPerSecond, ItemType.SCP2176));
	}
}

internal sealed class LightBulbVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 20f;
	public float RarityMultiplier { get; set; } = 0.8f;
	public int GrenadesPerSecond { get; set; } = 10;
}
