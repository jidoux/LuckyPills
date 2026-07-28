namespace LuckyPills.Effects;

internal sealed class LowGravity : LowGravityConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given low gravity for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Gravity = new Vector3(0f, -1f, -0f);
	}

	public void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Gravity = new Vector3(0f, -19.6f, 0f); // TODO if player dies in low gravity does it get reset upon respawn orrr
	}
}

internal class LowGravityConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 20f;
	public float RarityMultiplier { get; set; } = 1f;
}
