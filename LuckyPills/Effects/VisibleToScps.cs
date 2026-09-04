namespace LuckyPills.Effects;

internal sealed class VisibleToScps(VisibleToScpsConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've become visible to SCPs through walls for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.AnomalousTarget>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class VisibleToScpsConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 30;
	public int MaxDuration { get; set; } = 60;
	public ushort RarityWeight { get; set; } = 95;
}
