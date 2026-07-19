namespace LuckyPills.Effects;

internal record WallHacks : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been given wall hacks for {duration} seconds";
	protected override Duration Duration { get; } = new(10, 20);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.EnableEffect<CustomPlayerEffects.Scp1344>(duration: duration, addDuration: true); // TODO is ths detecterd or no
	}
}

