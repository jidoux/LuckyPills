namespace LuckyPills.Effects;

internal sealed record Dismemberment : DismembermentConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You have been dismembered...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.SeveredEyes>();
		player.EnableEffect<CustomPlayerEffects.SeveredHands>();
	}
}

internal record DismembermentConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
}
