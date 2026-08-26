namespace LuckyPills.Effects;

internal sealed class Australian(AustralianConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been converted to australian for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.Scale = new Vector3(1f, -1f, 1f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class AustralianConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
