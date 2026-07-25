namespace LuckyPills.Effects;

internal sealed record PocketDimension : PocketDimensionConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been sent to the pocket dimension";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Room? pocketDimensionRoom = Room.Get(MapGeneration.RoomName.Pocket).FirstOrDefault();
		if (pocketDimensionRoom is null) {
			Logger.Error("PocketDimensionRoom is null... this is a problem");
			return;
		}
		player.Position = pocketDimensionRoom.Position + Vector3.up; // Sometimes I teleported through the floor, the +1 is intended to fix.
	}
}

internal record PocketDimensionConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
