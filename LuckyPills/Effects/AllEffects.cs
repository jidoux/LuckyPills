namespace LuckyPills.Effects;

internal sealed class Allffects : AllEffectsConfig, IPillEffect {
	private static readonly List<IPillEffect> _effectCandidates = SharedCode.GetAllPillEffects()
		.Where(x => (x.Capabilities & EffectCapabilities.CandidateForGiveAll) == EffectCapabilities.CandidateForGiveAll)
		.ToList();

	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given (almost) every effect for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		IEnumerable<IPillEffect> effectsToUse = _effectCandidates.Where(x => x.IsEnabled(player));
		foreach (IPillEffect effect in effectsToUse) {
			SharedCode.EnablePillEffect(effect, player, duration);
		}
	}
}

internal class AllEffectsConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
