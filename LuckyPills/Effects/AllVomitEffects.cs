namespace LuckyPills.Effects;

internal sealed class AllVomitEffects : AllVomitEffectsConfig, IPillEffect {
	private static readonly List<IPillEffect> _effectCache = SharedCode.GetAllPillEffects()
		.Where(x => x.Capabilities.HasFlag(EffectCapabilities.VomitEffect))
		.ToList();

	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given every vomit effect for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		IEnumerable<IPillEffect> effectsToUse = _effectCache.Where(x => x.IsEnabled);
		foreach (IPillEffect effect in effectsToUse) {
			effect.OnEnabled(player, duration);
			MEC.Timing.CallDelayed(duration, () => effect.OnDisabled(player));
		}
	}
}

internal class AllVomitEffectsConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 1f;
}
