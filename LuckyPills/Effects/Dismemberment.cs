namespace LuckyPills.Effects;

internal sealed class Dismemberment(DismembermentConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You have been dismembered...";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.SeveredEyes>();
		player.EnableEffect<CustomPlayerEffects.SeveredHands>();
	}
}

internal sealed class DismembermentConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.4f; // I think this effect is just lame, so it should be kinda rare.
}
