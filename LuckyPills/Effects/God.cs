namespace LuckyPills.Effects;

internal sealed class God(GodConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given god mode for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
		player.IsGodModeEnabled = true;
	}

	public void OnDisabled(Player player) {
		player.IsGodModeEnabled = false;
	}
}

internal sealed class GodConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 30;
	public ushort RarityWeight { get; set; } = 90;
}
