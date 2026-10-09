namespace LuckyPills.Effects;

internal sealed class BecomeABomb(BecomeABombConfig config) : IPillEffect, IDebugPickPills {
	private static readonly HashSet<Player> _playersSpawningBombs = [];

	public bool IsEnabled(Player player) => !_playersSpawningBombs.Contains(player) && config.IsEnabled;
	public string DisplayText { get; } = "You've become a bomb for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		_playersSpawningBombs.Add(player);
		player.IsGodModeEnabled = true;
		Timing.RunCoroutine(SpawnBombs(player, duration, config.ExplosionsPerSecond));
	}

	public void OnDisabled(Player player) {
		_playersSpawningBombs.Remove(player);
		player.IsGodModeEnabled = false;
	}

	public void OnRoundEnd() {
		_playersSpawningBombs.Clear();
	}

	private static IEnumerator<float> SpawnBombs(Player player, float duration, int explosionsPerSecond) {
		float delayTime = 1f / explosionsPerSecond;
		for (int i = 0; i < duration * explosionsPerSecond; i++) {
			if (!_playersSpawningBombs.Contains(player)) {
				yield break;
			}
			player.BlowUp();
			yield return Timing.WaitForSeconds(delayTime);
		}
	}
}

internal sealed class BecomeABombConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 15;
	public ushort RarityWeight { get; set; } = 40; // This pill is currently the strongest one by far, so I'm making it fairly rare.
	public int ExplosionsPerSecond { get; set; } = 2;
}
