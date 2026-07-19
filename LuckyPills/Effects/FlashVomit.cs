namespace LuckyPills.Effects;

internal record FlashVomit : PillEffect {
	private const int _grenadesPerSecond = 10; // I'd prefer having this here rather than grenades class for potential fine tuning.

	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given flash vomit for {duration} seconds";
	protected override Duration Duration { get; } = new(10, 20);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		MEC.Timing.RunCoroutine(Grenades.RunGrenadeVomit(player, duration, _grenadesPerSecond, ItemType.GrenadeFlash));
	}
}
