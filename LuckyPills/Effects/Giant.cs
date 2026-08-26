namespace LuckyPills.Effects;

internal sealed class Giant(GiantConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been turned into a giant for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.Scale = new Vector3(2f, 2f, 2f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class GiantConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 8f;
	public float MaxDuration { get; set; } = 15f;
	public float RarityMultiplier { get; set; } = 0.8f;
}
