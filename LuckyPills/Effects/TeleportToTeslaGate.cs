namespace LuckyPills.Effects;

internal sealed class TeleportToTeslaGate(TeleportToTeslaGateConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => Map.Teslas.Count > 0 && config.IsEnabled;
	public string DisplayText { get; } = "You've been sent to a tesla gate";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		Tesla? tesla = Map.GetRandomTesla();
		if (tesla is null) {
			// May be possible with some crazy timing where this gets executed right when the map ends, but idk.
			Logger.Warn("No tesla gates found in the map. Odds are this should have been prevented before this point.");
			return;
		}
		player.Position = tesla.Position + Vector3.up;
	}
}

internal sealed class TeleportToTeslaGateConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
