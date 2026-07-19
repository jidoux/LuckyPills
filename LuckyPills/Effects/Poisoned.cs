namespace LuckyPills.Effects;

internal record Poisoned : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've poisoned yourself for {duration} seconds";
	protected override Duration Duration { get; } = new(10, 20);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Poisoned>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
