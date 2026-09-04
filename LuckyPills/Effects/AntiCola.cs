using CustomPlayerEffects;

namespace LuckyPills.Effects;

internal sealed class AntiColaEffect(AntiColaEffectConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given the anti-cola effect";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None; // can't be anything since the cola is "good effects" and "all effects" so player will die

	public void OnEnabled(Player player, int duration) {
		byte currentPlayerIntensity = 0;
		if (player.TryGetEffect<AntiScp207>(out AntiScp207? effect)) {
			currentPlayerIntensity = effect?.Intensity ?? 0; // my ide says this can return null somehow...
		}

		byte intensityToSet = currentPlayerIntensity;
		if (intensityToSet != byte.MaxValue) {
			intensityToSet += 1;
		}
		player.EnableEffect<AntiScp207>(intensity: intensityToSet, duration: float.MaxValue, addDuration: true);
	}
}

internal sealed class AntiColaEffectConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
