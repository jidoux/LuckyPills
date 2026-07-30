namespace LuckyPills.Effects;

internal sealed class Tantrum : TantrumConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "Oh...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		float sizeMultiplier = base.TantrumSizeMultiplier;
		if (Random.Range(0, 100) == 1) { // Rare chance to make it humongous (spelled that word first try too!)
			sizeMultiplier *= 100f;
		}
		TantrumHazard.Spawn(position: player.Position, rotation: Quaternion.identity, scale: Vector3.one * sizeMultiplier);
		// TODO make it slow the player
	}
}

internal class TantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float TantrumSizeMultiplier = 2f;
}
