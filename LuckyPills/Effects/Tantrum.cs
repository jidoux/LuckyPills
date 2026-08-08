using Hazards;
using Mirror;
using RelativePositioning;

namespace LuckyPills.Effects;

internal sealed class Tantrum : TantrumConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "Oh...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		TantrumEnvironmentalHazard? tantrum = UnityEngine.Object.Instantiate(SharedCode.GetPrefab<TantrumEnvironmentalHazard>());
		if (tantrum is null) {
			Logger.Error("Failed to instantiate Tantrum hazard for Tantrum pill effect... something changed?? Idk. Cancelling...");
			return;
		}
		tantrum.SynchronizedPosition = new RelativePosition(player.Position);

		NetworkServer.Spawn(tantrum.gameObject);
	}
}

internal class TantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float TantrumSizeMultiplier = 2f;
}
