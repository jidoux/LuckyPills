namespace LuckyPills.Effects;

internal sealed class TurnOffLights : TurnOffLightsConfig, IPillEffect {
	private bool _lightsAreOff = false; // Trying to avoid having this get triggered when the lights are already off.

	public new bool IsEnabled(Player player) => !_lightsAreOff && base.IsEnabled;
	public string DisplayText { get; } = "You've turned off the lights for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		_lightsAreOff = true;
		Map.TurnOffLights(duration);
		MEC.Timing.CallDelayed(duration, () => _lightsAreOff = false);
	}
}

internal class TurnOffLightsConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 300f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
