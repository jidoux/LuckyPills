namespace LuckyPills.Effects;

internal sealed class Handcuffed : HandcuffedConfig, IPillEffect {
	private readonly HashSet<Player> _cachedHandcuffedPlayers = [];

	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been handcuffed for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

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

internal class HandcuffedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 35f;
	public float MaxDuration { get; set; } = 70f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
