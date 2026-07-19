namespace LuckyPills.Effects;

internal record PocketDimension : PillEffect {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been sent to the pocket dimension";
	protected override Duration Duration { get; } = new(5, 10);

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Room? pocketDimensionRoom = Room.Get(MapGeneration.RoomName.Pocket).FirstOrDefault();
		if (pocketDimensionRoom is null) {
			Logger.Error("PocketDimensionRoom is null... this is a problem");
			return;
		}
		player.Position = pocketDimensionRoom.Position + new UnityEngine.Vector3(0, 0.1f, 0); // Sometimes I teleported through the floor... hoping this fixes that. TODO test this.
		player.EnableEffect<CustomPlayerEffects.Corroding>(intensity: byte.MaxValue, duration: duration, addDuration: false);
	}
}
