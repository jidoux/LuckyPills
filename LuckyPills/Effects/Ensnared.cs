namespace LuckyPills.Effects;

internal record Ensnared : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been ensnared for {duration} seconds";
	protected override Duration Duration { get; } = new(5, 10);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Ensnared>(intensity: 5, duration: duration, addDuration: true);
	}
}
