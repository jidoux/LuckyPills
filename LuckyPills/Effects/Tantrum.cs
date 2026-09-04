namespace LuckyPills.Effects;

internal sealed class Tantrum(TantrumConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "Oh...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		SpawnTantrum(player.Position);
	}
}

internal sealed class TantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 80;
}
