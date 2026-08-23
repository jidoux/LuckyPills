namespace LuckyPills.Effects;

internal sealed class PeriodicGrenades : PeriodicGrenadesConfig, IPillEffect {
	private static readonly HashSet<Player> _allGrenadePeriodicSpawns = [];

	public new bool IsEnabled(Player player) => !_allGrenadePeriodicSpawns.Contains(player) && base.IsEnabled;
	public string DisplayText { get; } = "You will uncontrollably spew out grenades...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		if (_allGrenadePeriodicSpawns.Add(player)) {
			MEC.Timing.RunCoroutine(SpawnGrenades(player, base.IntervalLowerBound, base.IntervalUpperBound));
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

internal class PeriodicGrenadesConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
	public float IntervalLowerBound { get; set; } = 10f;
	public float IntervalUpperBound { get; set; } = 40f;
}
