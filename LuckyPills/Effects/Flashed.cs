namespace LuckyPills.Effects;

internal record Flashed : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been flashed";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(5f, 5f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Flashed>(intensity: 5, duration: duration, addDuration: true);
	}
}
