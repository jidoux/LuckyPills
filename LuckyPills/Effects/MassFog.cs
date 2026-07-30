namespace LuckyPills.Effects;

internal sealed class MassFog : MassFogConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "Fog is spawning under you for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		MEC.Timing.RunCoroutine(SpawnMassFog(player, duration, base.Scp244PerSecond));
	}

	private static IEnumerator<float> SpawnMassFog(Player player, float duration, int scp244PerSecond) {
		float delayTime = 1f / scp244PerSecond;
		for (int i = 0; i < duration * scp244PerSecond; i++) {
			if (!player.IsAlive) {
				yield break;
			}
			SharedCode.SpawnScp244(player.Position);
			yield return MEC.Timing.WaitForSeconds(delayTime);
		}
	}
}

internal class MassFogConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 15f;
	public float MaxDuration { get; set; } = 38f;
	public float RarityMultiplier { get; set; } = 1f;
	public int Scp244PerSecond { get; set; } = 2;
}
