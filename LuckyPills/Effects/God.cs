namespace LuckyPills.Effects;

internal record God : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given god mode for {duration} seconds";
	protected override Duration Duration { get; } = new(5, 20);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsGodModeEnabled = true;
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsGodModeEnabled = false;
	}
}