namespace LuckyPills.Effects;

internal sealed record DeathCounter : DeathCounterConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => $"You will die in {base.SecondsToDeath} seconds";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
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
		ExplosiveGrenadeProjectile.SpawnActive(player.Position, ItemType.GrenadeHE, owner: player, timeOverride: 0f);
	}
}

internal record DeathCounterConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public int SecondsToDeath { get; set; } = 30;
}
