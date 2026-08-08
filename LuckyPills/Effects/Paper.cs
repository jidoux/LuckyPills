namespace LuckyPills.Effects;

internal sealed class Paper : PaperConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been turned into paper for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.Scale = new Vector3(1f, 1f, 0.01f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal class PaperConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 40f;
	public float RarityMultiplier { get; set; } = 1f;
}
