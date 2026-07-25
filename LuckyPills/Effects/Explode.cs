namespace LuckyPills.Effects;

internal sealed record Explode : ExplodeConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've spontaneously combusted";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.GrenadeHE, timeOverride: 0.001);
	}
}

internal record ExplodeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
