namespace LuckyPills.Effects;

// this one is kinda absurd... sometimes its fine, but you can easily get hard-stuck...
internal sealed class NoClip : NoClipConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given Noclip for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None; // Yeah not touching this one ahahaha

	public void OnEnabled(Player player, float duration) {
		player.IsNoclipEnabled = true;
	}

	public void OnDisabled(Player player) {
		player.IsNoclipEnabled = false;
	}
}

internal class NoClipConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 50f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
