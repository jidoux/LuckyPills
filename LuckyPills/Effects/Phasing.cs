namespace LuckyPills.Effects;

internal sealed class Phasing : PhasingConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've can phase through doors for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Ghostly>(intensity: 1, duration: duration, addDuration: true);
	}
}

internal class PhasingConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 30f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 1f;
}
