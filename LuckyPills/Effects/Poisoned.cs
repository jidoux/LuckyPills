namespace LuckyPills.Effects;

internal record Poisoned : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've poisoned yourself for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(10f, 20f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Poisoned>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
