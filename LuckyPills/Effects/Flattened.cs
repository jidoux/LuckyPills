namespace LuckyPills.Effects;

internal record Flattened : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been flattened for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(10f, 30f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new UnityEngine.Vector3(1f, 0.25f, 1f);
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = UnityEngine.Vector3.one;
	}
}