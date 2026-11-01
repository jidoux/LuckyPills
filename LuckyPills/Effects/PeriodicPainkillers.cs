namespace LuckyPills.Effects;

internal sealed class PeriodicPainkillers(PeriodicPainkillersConfig config) : IPillEffect, IDebugPickPills {
	private static readonly HashSet<Player> _allPeriodicPainkillerReceivers = [];

	public bool IsEnabled(Player player) => !_allPeriodicPainkillerReceivers.Contains(player) && config.IsEnabled;
	public string DisplayText { get; } = $"Until you die you will receive painkillers every {config.GivePillsInterval} seconds";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		if (_allPeriodicPainkillerReceivers.Add(player)) {
			Timing.RunCoroutine(GivePillsPeriodically(player, config.GivePillsInterval));
		}
		else {
			Logger.Error("Error with PeriodicPainkillers: player is already in the _allPeriodicPainkillerReceivers set.");
		}
	}

	// Called when player dies or whatever, so its reasonable to do this even though its technically infinite.
	public void OnDisabled(Player player) {
		_allPeriodicPainkillerReceivers.Remove(player);
	}

	public void OnRoundEnd() {
		_allPeriodicPainkillerReceivers.Clear();
	}

	private static IEnumerator<float> GivePillsPeriodically(Player player, int interval) {
		// Stop it if player dies, or the HashSet doesn't have player. The HashSet gets the player
		// removed if the player dies, escapes, or round ends.
		while (_allPeriodicPainkillerReceivers.Contains(player)) {
			if (player.Items.Count() >= 8) {
				Pickup.Create(ItemType.Painkillers, player.Position);
			}
			else {
				player.ForceEquip(ItemType.Painkillers);
			}
			yield return Timing.WaitForSeconds(interval);
		}
		_allPeriodicPainkillerReceivers.Remove(player); // I think its unnecessary, but its defensive.
	}
}

internal sealed class PeriodicPainkillersConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 70;
	public int GivePillsInterval { get; set; } = 30;
}
