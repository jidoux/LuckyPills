namespace LuckyPills.Effects;

internal sealed class Giant(GiantConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been turned into a giant for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		player.Scale = new Vector3(2f, 2f, 2f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class GiantConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 8;
	public int MaxDuration { get; set; } = 18;
	public ushort RarityWeight { get; set; } = 80;
}
