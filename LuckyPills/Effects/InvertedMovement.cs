namespace LuckyPills.Effects;

internal sealed class InvertedMovement(InvertedMovementConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given inverted movement for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		// Idk why this works, but yeah giving max slowness inverts movement??? Lol
		// 209 was too fast. 200 felt like the normal player speed... my understanding is that its some
		// overflow scenario.
		player.EnableEffect<CustomPlayerEffects.Slowness>(intensity: 200, duration: duration, addDuration: true);
	}
}

internal sealed class InvertedMovementConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 20;
	public int MaxDuration { get; set; } = 140;
	public ushort RarityWeight { get; set; } = 95;
}
