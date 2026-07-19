namespace LuckyPills.Effects;

internal record Ensnared : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been ensnared for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(5f, 10f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Ensnared>(intensity: 5, duration: duration, addDuration: true);
	}
}
