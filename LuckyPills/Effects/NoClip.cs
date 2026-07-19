namespace LuckyPills.Effects;

internal record NoClip : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given Noclip for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(4f, 10f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsNoclipEnabled = true;
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsNoclipEnabled = false;
	}
}