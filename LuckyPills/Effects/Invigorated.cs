namespace LuckyPills.Effects;

internal sealed class Invigorated : InvigoratedConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been invigorated for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.Invigorated>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class InvigoratedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 120f;
	public float MaxDuration { get; set; } = 300f;
	public float RarityMultiplier { get; set; } = 1f;
}
