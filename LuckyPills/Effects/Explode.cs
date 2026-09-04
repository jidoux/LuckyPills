namespace LuckyPills.Effects;

internal sealed class Explode(ExplodeConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've spontaneously combusted";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.BlowUp();
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.SCP2176, owner: player, timeOverride: 0f);
	}
}

internal sealed class ExplodeConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 85;
}
