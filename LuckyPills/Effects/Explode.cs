namespace LuckyPills.Effects;

internal sealed class Explode(ExplodeConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've spontaneously combusted";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.BlowUp();
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.SCP2176, owner: player, timeOverride: 0f);
	}
}

internal sealed class ExplodeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.85f;
}
