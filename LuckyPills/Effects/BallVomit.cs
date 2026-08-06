namespace LuckyPills.Effects;

internal sealed class BallVomit : BallVomitConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given ball vomit for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.VomitEffect | EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		int grenadesPerSecond = base.GrenadesPerSecond;
		if (Random.Range(0, 100) == 1) { // Rare chance to spawn a whole lot more
			grenadesPerSecond *= 10;
		}
		MEC.Timing.RunCoroutine(SharedCode.RunGrenadeVomit(player, duration, grenadesPerSecond, ItemType.SCP018));
	}
}

internal class BallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 20f;
	public float RarityMultiplier { get; set; } = 1f;
	public int GrenadesPerSecond { get; set; } = 10;
}
