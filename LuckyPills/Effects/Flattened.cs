namespace LuckyPills.Effects;

internal sealed class Flattened(FlattenedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been flattened for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.Scale = new Vector3(1f, 0.25f, 1f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class FlattenedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 120f;
	public float RarityMultiplier { get; set; } = 0.95f;
}
