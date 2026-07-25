namespace LuckyPills.Effects;

internal sealed record Bleeding : BleedingConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given bleeding for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Bleeding>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}


internal record BleedingConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 6f;
	public float MaxDuration { get; set; } = 12f;
	public float RarityMultiplier { get; set; } = 1f;
}
