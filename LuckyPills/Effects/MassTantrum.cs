namespace LuckyPills.Effects;

internal sealed class MassTantrum : MassTantrumConfig, IPillEffect {
	private static readonly HashSet<Player> _allTantrumMassSpawns = [];

	public new bool IsEnabled(Player player) => !_allTantrumMassSpawns.Contains(player) && base.IsEnabled;
	public string DisplayText => "Your stomach begins to quake...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		if (_allTantrumMassSpawns.Add(player)) {
			MEC.Timing.RunCoroutine(SpawnTantrums(player, base.TantrumsPerSecondMin, base.TantrumsPerSecondMax));
		}
		else {
			Logger.Error("Error with MassTantrum: player is already in the _allTantrumMassSpawns set.");
		}
	}

	// Called when player dies or whatever, so its reasonable to do this even though its technically infinite.
	public void OnDisabled(Player player) {
		_allTantrumMassSpawns.Remove(player);
	}

	public void OnRoundEnd() {
		_allTantrumMassSpawns.Clear();
	}

	private static IEnumerator<float> SpawnTantrums(Player player, float tantrumsPerSecondMin, float tantrumsPerSecondMax) {
		yield return MEC.Timing.WaitForSeconds(Random.Range(tantrumsPerSecondMin, tantrumsPerSecondMax));
		// Stop it if player dies, or the HashSet doesn't have player. The HashSet gets the player
		// removed if the player dies, escapes, or round ends.
		while (player.IsAlive && _allTantrumMassSpawns.Contains(player)) {
			SpawnTantrum(player.Position);
			yield return MEC.Timing.WaitForSeconds(Random.Range(tantrumsPerSecondMin, tantrumsPerSecondMax));
		}
		_allTantrumMassSpawns.Remove(player); // I think its unnecessary, but its defensive.
	}
}

internal class MassTantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
	public float TantrumsPerSecondMin { get; set; } = 30f;
	public float TantrumsPerSecondMax { get; set; } = 180f;
}
