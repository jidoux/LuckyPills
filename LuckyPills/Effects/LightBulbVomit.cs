namespace LuckyPills.Effects;

internal sealed class LightBulbVomit :  LightBulbVomitConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given light bulb vomit for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.VomitEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		MEC.Timing.RunCoroutine(SharedCode.RunGrenadeVomit(player, duration, base.GrenadesPerSecond, ItemType.SCP2176));
	}
}

internal class LightBulbVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 20f;
	public float RarityMultiplier { get; set; } = 1f;
	public int GrenadesPerSecond { get; set; } = 10;
}
