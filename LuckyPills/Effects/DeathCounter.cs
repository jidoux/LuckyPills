namespace LuckyPills.Effects;

internal sealed class DeathCounter(DeathCounterConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText => $"You will die in {config.SecondsToDeath} seconds";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
		MEC.Timing.RunCoroutine(DeathCountdown(player, config.SecondsToDeath));
	}

	private static IEnumerator<float> DeathCountdown(Player player, int secondsToDeath) {
		// Atypical reverse for loop since its just meant to count down seconds, so it starts from base.SecondsToDeath - 1 and ends at 1
		for (int i = secondsToDeath; i > 0; i--) {
			if (!player.IsAlive) {
				yield break;
			}
			player.SendHint($"{i}...", duration: 1f);
			yield return MEC.Timing.WaitForSeconds(1);
		}
		// I experimented with Player.Kill() and actually blowing them up, and they preferred the explosive grenade.
		player.BlowUp();
	}
}

internal sealed class DeathCounterConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 90;
	public int SecondsToDeath { get; set; } = 30;
}
