namespace LuckyPills.Effects;

internal sealed class DeathCounter(DeathCounterConfig config) : IPillEffect {
	private static HashSet<Player> _playersCurrentlyDying = [];

	public bool IsEnabled(Player player) => config.IsEnabled && !_playersCurrentlyDying.Contains(player);
	public string DisplayText => $"You will die in {config.SecondsToDeath} seconds";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		Timing.RunCoroutine(DeathCountdown(player, config.SecondsToDeath));
	}

	public void OnRoundEnd() {
		// Probably this is unnecessary, but I figure its defensive at least.
		_playersCurrentlyDying = [];
	}

	private static IEnumerator<float> DeathCountdown(Player player, int secondsToDeath) {
		try {
			_playersCurrentlyDying.Add(player);
			// Atypical reverse for loop since its just meant to count down seconds, so it starts from base.SecondsToDeath - 1 and ends at 1
			for (int i = secondsToDeath; i > 0; i--) {
				if (!player.IsAlive) {
					yield break;
				}
				player.SendHint($"{i}...", duration: 1f);
				yield return Timing.WaitForSeconds(1);
			}
			// I experimented with Player.Kill() and actually blowing them up, and they preferred the explosive grenade.
			player.BlowUp();
		}
		finally {
			_playersCurrentlyDying.Remove(player);
		}
	}
}

internal sealed class DeathCounterConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 90;
	public int SecondsToDeath { get; set; } = 30;
}
