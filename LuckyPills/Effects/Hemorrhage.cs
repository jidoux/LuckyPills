namespace LuckyPills.Effects;

internal record Hemorrhage : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've begun to hemorrhage for the next {duration} seconds";
	protected override Duration Duration { get; } = new(5, 10);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Hemorrhage>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
