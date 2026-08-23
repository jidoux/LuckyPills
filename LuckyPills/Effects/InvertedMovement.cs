namespace LuckyPills.Effects;

internal sealed class InvertedMovement : InvertedMovementConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "You've been given inverted movement for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		// Idk why this works, but yeah giving max slowness inverts movement??? Lol
		// 200 they were too slow, 209 too fast
		player.EnableEffect<CustomPlayerEffects.Slowness>(intensity: 200, duration: duration, addDuration: true);
	}
}

internal class InvertedMovementConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 30f;
	public float MaxDuration { get; set; } = 120f;
	public float RarityMultiplier { get; set; } = 1f;
}
