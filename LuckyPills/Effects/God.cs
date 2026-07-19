namespace LuckyPills.Effects;

internal record God : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given god mode for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(5f, 20f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsGodModeEnabled = true;
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsGodModeEnabled = false;
	}
}