namespace LuckyPills.Effects;

internal sealed class Tantrum(TantrumConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "Oh...";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		SpawnTantrum(player.Position);
	}
}

internal sealed class TantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.8f;
	public float TantrumSizeMultiplier = 2f;
}
