using CustomPlayerEffects;

namespace LuckyPills.Effects;

internal sealed class AntiColaEffect(AntiColaEffectConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given the anti-cola effect";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None; // can't be anything since the cola is "good effects" and "all effects" so player will die

	public void OnEnabled(Player player, float duration) {
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
	public float RarityMultiplier { get; set; } = 1f;
}
