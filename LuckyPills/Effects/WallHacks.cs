namespace LuckyPills.Effects;

internal sealed class WallHacks : WallHacksConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given wall hacks for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Scp1344>(intensity: 1, duration: duration, addDuration: true);  // TODO is ths detecterd or no
	}
}

internal class WallHacksConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 60f;
	public float MaxDuration { get; set; } = 120f;
	public float RarityMultiplier { get; set; } = 1f;
}
