namespace LuckyPills.Effects;

internal sealed class Paper(PaperConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been turned into paper for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.Scale = new Vector3(1f, 1f, 0.01f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class PaperConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 100;
	public ushort RarityWeight { get; set; } = 95;
}
