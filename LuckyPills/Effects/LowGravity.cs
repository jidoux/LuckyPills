namespace LuckyPills.Effects;

internal record LowGravity : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given low gravity for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(10f, 20f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Gravity = new UnityEngine.Vector3(0f, -1f, -0f);
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Gravity = new UnityEngine.Vector3(0f, -19.6f, 0f);
	}
}
