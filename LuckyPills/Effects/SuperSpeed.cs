namespace LuckyPills.Effects;

internal record SuperSpeed : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given super speed for {duration} seconds";

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.MovementBoost>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}