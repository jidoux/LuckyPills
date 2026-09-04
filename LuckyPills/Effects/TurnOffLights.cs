namespace LuckyPills.Effects;

internal sealed class TurnOffLights(TurnOffLightsConfig config) : IPillEffect {
	private bool _lightsAreOff = false; // Trying to avoid having this get triggered when the lights are already off.

	public bool IsEnabled(Player player) => !_lightsAreOff && config.IsEnabled;
	public string DisplayText { get; } = "You've turned off the lights for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent; // I was told this effect was quite fun/cool experience.

	public void OnEnabled(Player player, int duration) {
		_lightsAreOff = true;
		Map.TurnOffLights(duration);
		MEC.Timing.CallDelayed(duration, () => _lightsAreOff = false);
	}
}

internal sealed class TurnOffLightsConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 300;
	public ushort RarityWeight { get; set; } = 50;
}
