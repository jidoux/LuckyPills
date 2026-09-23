namespace LuckyPills.Effects;

internal sealed class FlashVomit(FlashVomitConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given flash vomit for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	// Removed from EffectCapabilities.CandidateForGiveAll because people didn't think it was adding to the fun.
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.VomitEffect;

	public void OnEnabled(Player player, int duration) {
		MEC.Timing.RunCoroutine(RunGrenadeVomit(player, duration, config.GrenadesPerSecond, ItemType.GrenadeFlash));
	}
}

internal sealed class FlashVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 20;
	public ushort RarityWeight { get; set; } = 80;
	public int GrenadesPerSecond { get; set; } = 10;
}
