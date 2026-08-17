namespace LuckyPills.Effects;

internal sealed class DeathCounter : DeathCounterConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => $"You will die in {base.SecondsToDeath} seconds";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		MEC.Timing.RunCoroutine(DeathCountdown(player, base.SecondsToDeath));
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
		// TODO test this instead... might be more fun since less death for others idk tbh
		player.Kill();
		// ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.GrenadeHE, owner: player, timeOverride: 0f);
	}
}

internal class DeathCounterConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.9f;
	public int SecondsToDeath { get; set; } = 30;
}
