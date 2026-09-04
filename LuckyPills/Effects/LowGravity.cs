namespace LuckyPills.Effects;

internal sealed class LowGravity(LowGravityConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given low gravity for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.Gravity = new Vector3(0f, -1f, -0f);
	}

	public void OnDisabled(Player player) {
		player.Gravity = new Vector3(0f, -19.6f, 0f);
	}
}

internal sealed class LowGravityConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 40;
	public ushort RarityWeight { get; set; } = 95;
}
