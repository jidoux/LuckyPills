namespace LuckyPills.Effects;

internal record Australian : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been converted to australian for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(10f, 30f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new UnityEngine.Vector3(1f, -1f, 1f); // TODO this doesnt make them float up a bit, right? It might... fine tune mby, idk.
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Position += UnityEngine.Vector3.up;
		player.Scale = UnityEngine.Vector3.one;
	}
}
