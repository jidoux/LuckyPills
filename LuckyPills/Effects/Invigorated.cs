namespace LuckyPills.Effects;

internal record Invigorated : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been invigorated for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(40f, 100f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Invigorated>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
