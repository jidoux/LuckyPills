namespace LuckyPills.Effects;

internal sealed class Tantrum : TantrumConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "Oh...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		SpawnTantrum(player.Position);
	}
}

internal class TantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.8f;
	public float TantrumSizeMultiplier = 2f;
}
