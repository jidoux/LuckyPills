namespace LuckyPills.Effects;

internal sealed class AllGoodEffects : AllGoodEffectsConfig, IPillEffect {
	private static readonly Lazy<IPillEffect[]> _effectCandidates = new(() => {
		// I did this mainly because I was scared of static initialization order issues with my singleton AllPillEffects,
		// but this also may be better to not allocate this static field, to be honest.
		return AllPillEffects
			.Where(x => (x.Capabilities & EffectCapabilities.GoodEffect) == EffectCapabilities.GoodEffect)
			.ToArray();
	});

	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "You've been given every good effect for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (IPillEffect effect in _effectCandidates.Value) {
			if (effect.IsEnabled(player)) {
				EnablePillEffect(effect, player, duration);
			}
		}
	}
}

internal class AllGoodEffectsConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 0.1f;
}
