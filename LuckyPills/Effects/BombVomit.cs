namespace LuckyPills.Effects;

internal sealed class BombVomit(BombVomitConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given bomb vomit for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.VomitEffect | EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		MEC.Timing.RunCoroutine(RunGrenadeVomit(player, duration, config.GrenadesPerSecond, ItemType.GrenadeHE));
	}
}

internal sealed class BombVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 20;
	public ushort RarityWeight { get; set; } = 90;
	public int GrenadesPerSecond { get; set; } = 10;
}
