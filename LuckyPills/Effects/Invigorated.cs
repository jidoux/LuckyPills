namespace LuckyPills.Effects;

internal sealed class Invigorated : InvigoratedConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been invigorated for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Invigorated>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class InvigoratedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 40f;
	public float MaxDuration { get; set; } = 100f;
	public float RarityMultiplier { get; set; } = 1f;
}
