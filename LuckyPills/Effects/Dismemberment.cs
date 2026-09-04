namespace LuckyPills.Effects;

internal sealed class Dismemberment(DismembermentConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You have been dismembered...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.EnableEffect<CustomPlayerEffects.SeveredEyes>();
		player.EnableEffect<CustomPlayerEffects.SeveredHands>();
	}
}

internal sealed class DismembermentConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 40; // I think this effect is just lame, so it should be kinda rare.
}
