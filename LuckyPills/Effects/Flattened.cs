namespace LuckyPills.Effects;

internal sealed class Flattened : FlattenedConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been flattened for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new Vector3(1f, 0.25f, 1f);
	}

	public void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = Vector3.one;
	}
}

internal class FlattenedConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 1f;
}
