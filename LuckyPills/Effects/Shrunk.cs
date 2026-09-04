namespace LuckyPills.Effects;

internal sealed class Shrunk(ShrunkConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been shrunk for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.Scale = new Vector3(0.2f, 0.2f, 0.2f);
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}
}

internal sealed class ShrunkConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 60;
	public ushort RarityWeight { get; set; } = 95;
}
