namespace LuckyPills.Effects;

// this one is kinda absurd... sometimes its fine, but you can easily get hard-stuck...
internal sealed class NoClip(NoClipConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given Noclip for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None; // Yeah not touching this one ahahaha

	public void OnEnabled(Player player, float duration) {
		player.IsNoclipEnabled = true;
	}

	public void OnDisabled(Player player) {
		player.IsNoclipEnabled = false;
	}
}

internal sealed class NoClipConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 50f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
