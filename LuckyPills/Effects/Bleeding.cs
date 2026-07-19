namespace LuckyPills.Effects;

internal record Bleeding : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given bleeding for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(6f, 12f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Bleeding>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
