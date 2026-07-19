namespace LuckyPills.Effects;

internal record Explode : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've spontaneously combusted";

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.GrenadeHE, timeOverride: 0.01); // TODO test this
	}
}
