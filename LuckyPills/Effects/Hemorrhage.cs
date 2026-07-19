namespace LuckyPills.Effects;

internal record Hemorrhage : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've begun to hemorrhage for the next {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(5f, 10f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Hemorrhage>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
