namespace LuckyPills.Effects;

internal sealed class Invigorated(InvigoratedConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been invigorated for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.Invigorated>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal sealed class InvigoratedConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 120;
	public int MaxDuration { get; set; } = 300;
	public ushort RarityWeight { get; set; } = 95;
}
