namespace LuckyPills.Effects;

internal sealed class Australian(AustralianConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been converted to australian for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.Scale = new Vector3(1f, -1f, 1f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class AustralianConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 60;
	public ushort RarityWeight { get; set; } = 95;
}
