namespace LuckyPills.Effects;

internal sealed class LowGravity(LowGravityConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled && player.HasDefaultScaleAndGravity();
	public string DisplayText { get; } = "You've been given low gravity for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	// Should not have EffectCapabilities.CandidateForGiveAll or anything like that as giving them this and adjusting their size is problematic.
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.Gravity = new Vector3(0f, -1f, -0f);
	}

	public void OnDisabled(Player player) {
		player.Gravity = DefaultPlayerGravity;
	}
}

internal sealed class LowGravityConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 40;
	public ushort RarityWeight { get; set; } = 95;
}
