namespace LuckyPills.Effects;

internal sealed record Invisible : InvisibleConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been turned invisible for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Invisible>(duration: duration, addDuration: true);
	}
}

internal record InvisibleConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 19f;
	public float MaxDuration { get; set; } = 35f;
	public float RarityMultiplier { get; set; } = 1f;
}
