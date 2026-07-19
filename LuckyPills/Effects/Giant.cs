namespace LuckyPills.Effects;

internal record Giant : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been turned into a giant for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(8f, 15f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new UnityEngine.Vector3(2f, 2f, 2f);
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = UnityEngine.Vector3.one;
	}
}
