namespace LuckyPills.Effects;

internal sealed class VisibleToScps : VisibleToScpsConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "You've become visible to SCPs through walls for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		player.EnableEffect<CustomPlayerEffects.AnomalousTarget>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}

internal class VisibleToScpsConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 30f;
	public float MaxDuration { get; set; } = 60f;
	public float RarityMultiplier { get; set; } = 1f;
}
