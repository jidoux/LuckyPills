namespace LuckyPills.Effects;

internal sealed class PeriodicGrenades(PeriodicGrenadesConfig config) : IPillEffect, IDebugPickPills {
	private static readonly HashSet<Player> _allGrenadePeriodicSpawns = [];

	public bool IsEnabled(Player player) => !_allGrenadePeriodicSpawns.Contains(player) && config.IsEnabled;
	public string DisplayText { get; } = "You will uncontrollably spew out grenades...";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		if (_allGrenadePeriodicSpawns.Add(player)) {
			MEC.Timing.RunCoroutine(SpawnGrenades(player, config.IntervalLowerBound, config.IntervalUpperBound));
		}
		else {
			Logger.Error("Error with PeriodicGrenades: player is already in the _allGrenadePeriodicSpawns set.");
		}
	}

	// Called when player dies or whatever, so its reasonable to do this even though its technically infinite.
	public void OnDisabled(Player player) {
		_allGrenadePeriodicSpawns.Remove(player);
	}

	public void OnRoundEnd() {
		_allGrenadePeriodicSpawns.Clear();
	}

	private static IEnumerator<float> SpawnGrenades(Player player, float intervalLowBound, float intervalUpperBound) {
		yield return MEC.Timing.WaitForSeconds(Random.Range(intervalLowBound, intervalUpperBound));
		// Stop it if player dies, or the HashSet doesn't have player. The HashSet gets the player
		// removed if the player dies, escapes, or round ends.
		while (_allGrenadePeriodicSpawns.Contains(player)) {
			SpawnThrownExplosive(player.ReferenceHub, ItemType.GrenadeHE);
			yield return MEC.Timing.WaitForSeconds(Random.Range(intervalLowBound, intervalUpperBound));
		}
		_allGrenadePeriodicSpawns.Remove(player); // I think its unnecessary, but its defensive.
	}
}

internal sealed class PeriodicGrenadesConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
	public float IntervalLowerBound { get; set; } = 5f;
	public float IntervalUpperBound { get; set; } = 30f;
}
