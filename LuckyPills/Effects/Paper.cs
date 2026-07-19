namespace LuckyPills.Effects;

internal record Paper : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been turned into paper for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(12f, 36f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new UnityEngine.Vector3(1f, 1f, 0.01f);
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = UnityEngine.Vector3.one;
	}
}
