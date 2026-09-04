namespace LuckyPills.Effects;

internal sealed class CardiacArrest(CardiacArrestConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given cardiac arrest for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.CardiacArrest>(intensity: 1, duration: duration, addDuration: true);
	}
}

internal sealed class CardiacArrestConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 10;
	public ushort RarityWeight { get; set; } = 90;
}
