namespace LuckyPills.Effects;

internal sealed class WallHacks(WallHacksConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given wall hacks for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		// I have no clue what Scp1344Detected is... I gave it to a player and noticed nothing visually from my alt account.
		player.EnableEffect<CustomPlayerEffects.Scp1344>(intensity: 1, duration: duration, addDuration: true);
	}
}
internal sealed class WallHacksConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 60;
	public int MaxDuration { get; set; } = 240;
	public ushort RarityWeight { get; set; } = 95;
}
