namespace LuckyPills.Effects;

internal record Flashed : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been flashed";
	protected override Duration Duration { get; } = new(5, 5);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Flashed>(intensity: 5, duration: duration, addDuration: true);
	}
}
