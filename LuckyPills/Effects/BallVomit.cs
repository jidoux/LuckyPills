namespace LuckyPills.Effects;

internal sealed class BallVomit(BallVomitConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given ball vomit for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.VomitEffect | EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		int grenadesPerSecond = config.GrenadesPerSecond;
		if (Random.Range(0, 100) == 1) { // Rare chance to spawn a whole lot more
			grenadesPerSecond *= 10;
		}
		MEC.Timing.RunCoroutine(RunGrenadeVomit(player, duration, grenadesPerSecond, ItemType.SCP018));
	}
}

internal sealed class BallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 20;
	public ushort RarityWeight { get; set; } = 90;
	public int GrenadesPerSecond { get; set; } = 10;
}
