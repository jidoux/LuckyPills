namespace LuckyPills.Effects;

internal record Concussed : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been concussed for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(10f, 20f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Concussed>(intensity: byte.MaxValue, duration: duration, addDuration: true);
		player.EnableEffect<CustomPlayerEffects.Blurred>(intensity: byte.MaxValue, duration: duration, addDuration: true);
	}
}
