namespace LuckyPills.Effects;

internal sealed class CardiacArrest : CardiacArrestConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given cardiac arrest for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.CardiacArrest>(intensity: 1, duration: duration, addDuration: true);
	}
}

internal class CardiacArrestConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 10f;
	public float RarityMultiplier { get; set; } = 0.9f;
}
