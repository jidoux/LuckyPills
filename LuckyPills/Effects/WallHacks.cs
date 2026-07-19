namespace LuckyPills.Effects;

internal record WallHacks : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given wall hacks for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(60f, 120f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Scp1344>(intensity: 1, duration: duration, addDuration: true); // TODO is ths detecterd or no
	}
}
