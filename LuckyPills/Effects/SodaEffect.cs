using CustomPlayerEffects;

namespace LuckyPills;

internal sealed record SodaEffect : SodaEffectConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given the soda effect";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");

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

internal record SodaEffectConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
