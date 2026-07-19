namespace LuckyPills.Effects;

internal record PocketDimension : PillEffect, IDebugPickPills {
	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been sent to the pocket dimension";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(5f, 10f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Room? pocketDimensionRoom = Room.Get(MapGeneration.RoomName.Pocket).FirstOrDefault();
		if (pocketDimensionRoom is null) {
			Logger.Error("PocketDimensionRoom is null... this is a problem");
			return;
		}
		player.Position = pocketDimensionRoom.Position + UnityEngine.Vector3.up; // Sometimes I teleported through the floor, the +1 is intended to fix.
		player.EnableEffect<CustomPlayerEffects.Corroding>(intensity: byte.MaxValue, duration: duration, addDuration: false);
	}
}
