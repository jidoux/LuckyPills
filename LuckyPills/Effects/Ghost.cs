namespace LuckyPills.Effects;

internal record Ghost : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've become a ghost for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(30f, 60f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Ghostly>(intensity: 1, duration: duration, addDuration: true);
	}
}
