using CustomPlayerEffects;

namespace LuckyPills.Effects;

internal sealed class SodaEffect(SodaEffectConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given the soda effect";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		byte currentPlayerIntensity = 0;
		if (player.TryGetEffect<Scp207>(out Scp207? effect)) {
			currentPlayerIntensity = effect?.Intensity ?? 0; // my ide says this can return null somehow...
		}

		byte intensityToSet = currentPlayerIntensity;
		if (intensityToSet != byte.MaxValue) {
			intensityToSet += 1;
		}
		player.EnableEffect<Scp207>(intensity: intensityToSet, duration: float.MaxValue, addDuration: true);
	}
}

internal sealed class SodaEffectConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
