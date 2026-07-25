namespace LuckyPills.Effects;

internal sealed record Ensnared : EnsnaredConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been ensnared for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Ensnared>(intensity: 5, duration: duration, addDuration: true);
	}
}

internal record EnsnaredConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 5f;
	public float MaxDuration { get; set; } = 10f;
	public float RarityMultiplier { get; set; } = 1f;
}
