namespace LuckyPills.Effects;

internal sealed class Explode : ExplodeConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've spontaneously combusted";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.GrenadeHE, owner: player, timeOverride: 0f);
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.SCP2176, owner: player, timeOverride: 0f);
	}
}

internal class ExplodeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.9f;
}
