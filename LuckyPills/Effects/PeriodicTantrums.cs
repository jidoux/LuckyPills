namespace LuckyPills.Effects;

internal sealed class PeriodicTantrums : PeriodicTantrumsConfig, IPillEffect {
	private static readonly HashSet<Player> _allTantrumPeriodicSpawns = [];

	public new bool IsEnabled(Player player) => !_allTantrumPeriodicSpawns.Contains(player) && base.IsEnabled;
	public string DisplayText { get; } = "Your stomach begins to quake...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		if (_allTantrumPeriodicSpawns.Add(player)) {
			MEC.Timing.RunCoroutine(SpawnTantrums(player, base.IntervalLowerBound, base.IntervalUpperBound));
		}
		else {
			Logger.Error("Error with PeriodicTantrums: player is already in the _allTantrumPeriodicSpawns set.");
		}
	}

	// Called when player dies or whatever, so its reasonable to do this even though its technically infinite.
	public void OnDisabled(Player player) {
		_allTantrumPeriodicSpawns.Remove(player);
	}

	public void OnRoundEnd() {
		_allTantrumPeriodicSpawns.Clear();
	}

	private static IEnumerator<float> SpawnTantrums(Player player, float intervalLowBound, float intervalUpperBound) {
		yield return MEC.Timing.WaitForSeconds(Random.Range(intervalLowBound, intervalUpperBound));
		// Stop it if player dies, or the HashSet doesn't have player. The HashSet gets the player
		// removed if the player dies, escapes, or round ends.
		while (_allTantrumPeriodicSpawns.Contains(player)) {
			SpawnTantrum(player.Position);
			yield return MEC.Timing.WaitForSeconds(Random.Range(intervalLowBound, intervalUpperBound));
		}
		_allTantrumPeriodicSpawns.Remove(player); // I think its unnecessary, but its defensive.
	}
}

internal class PeriodicTantrumsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
	public float IntervalLowerBound { get; set; } = 30f;
	public float IntervalUpperBound { get; set; } = 90f;
}
