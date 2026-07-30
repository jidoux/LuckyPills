namespace LuckyPills.Effects;

internal sealed class God : GodConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given god mode for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.IsGodModeEnabled = true;
	}

	public void OnDisabled(Player player) {
		player.IsGodModeEnabled = false;
	}
}

internal class GodConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 25f;
	public float RarityMultiplier { get; set; } = 1f;
}
