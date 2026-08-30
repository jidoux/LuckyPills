namespace LuckyPills.Effects;

internal sealed class MassFog(MassFogConfig config) : IPillEffect {
	private static readonly HashSet<Player> _playersSpawningMassFog = [];

	public bool IsEnabled(Player player) => !_playersSpawningMassFog.Contains(player) && config.IsEnabled;
	public string DisplayText { get; } = "Fog is spawning under you for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, float duration) {
		_playersSpawningMassFog.Add(player);
		MEC.Timing.RunCoroutine(SpawnMassFog(player, duration, config.Scp244PerSecond));
	}

	public void OnDisabled(Player player) {
		_playersSpawningMassFog.Remove(player);
	}

	public void OnRoundEnd() {
		_playersSpawningMassFog.Clear();
	}

	private static IEnumerator<float> SpawnMassFog(Player player, float duration, float scp244PerSecond) {
		float delayTime = 1f / scp244PerSecond;
		for (int i = 0; i < duration * scp244PerSecond; i++) {
			if (!_playersSpawningMassFog.Contains(player)) {
				yield break;
			}
			SpawnScp244(player.Position);
			yield return MEC.Timing.WaitForSeconds(delayTime);
		}
	}
}

internal sealed class MassFogConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 38f;
	public float RarityMultiplier { get; set; } = 1f;
	public float Scp244PerSecond { get; set; } = 2f;
}
