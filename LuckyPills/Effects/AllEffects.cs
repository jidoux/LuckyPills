namespace LuckyPills.Effects;

internal sealed class AllEffects(AllEffectsConfig config) : IPillEffect {
	private static readonly Lazy<IPillEffect[]> _effectCandidates = new(() => {
		// I did this mainly because I was scared of static initialization order issues with my singleton AllPillEffects,
		// but this also may be better to not allocate this static field, to be honest.
		return AllPillEffects
			.Where(x => (x.Capabilities & EffectCapabilities.CandidateForGiveAll) == EffectCapabilities.CandidateForGiveAll)
			.ToArray();
	});

	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given (almost) every effect for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		foreach (IPillEffect effect in _effectCandidates.Value) {
			if (effect.IsEnabled(player)) {
				EnablePillEffect(effect, player, duration);
			}
		}
	}
}

internal sealed class AllEffectsConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 20;
	public int MaxDuration { get; set; } = 30;
	public ushort RarityWeight { get; set; } = 10;
}
