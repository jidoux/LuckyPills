namespace LuckyPills.Effects;

internal sealed class Giant : GiantConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been turned into a giant for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new Vector3(2f, 2f, 2f);
	}

	public void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = Vector3.one;
	}
}

internal class GiantConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 8f;
	public float MaxDuration { get; set; } = 15f;
	public float RarityMultiplier { get; set; } = 1f;
}
