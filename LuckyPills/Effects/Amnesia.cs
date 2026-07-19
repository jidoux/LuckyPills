namespace LuckyPills.Effects;

internal record Amnesia : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given amnesia for {duration} seconds";
	protected override Duration Duration { get; } = new(10, 20);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.AmnesiaVision>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
