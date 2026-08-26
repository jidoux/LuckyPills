namespace LuckyPills.Effects;

internal sealed class Handcuffed(HandcuffedConfig config) : IPillEffect {
	private readonly HashSet<Player> _cachedHandcuffedPlayers = [];

	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been handcuffed for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		if (player.IsDisarmed || !player.IsAlive) {
			// Just being defensive... not sure if these are even possible tbh.
			return;
		}
		if (_cachedHandcuffedPlayers.Add(player)) {
			player.IsDisarmed = true;
			player.DropAllItems();
			// TODO maybe figure out howto make it say the player was disarmed by themselves or  something idk.
			//player.DisarmedBy = player;
			//player.Inventory.SetDisarmedStatus(player.Inventory);
		}
	}

	public void OnDisabled(Player player) {
		// I'd think its fine to not check if the player is alive here... probably doesn't even matter tbh.
		if (_cachedHandcuffedPlayers.Remove(player)) {
			player.IsDisarmed = false;
		}
	}
}

internal sealed class HandcuffedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 35f;
	public float MaxDuration { get; set; } = 70f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
