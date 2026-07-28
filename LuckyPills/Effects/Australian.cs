namespace LuckyPills.Effects;

internal sealed class Australian : AustralianConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been converted to australian for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Scale = new Vector3(1f, -1f, 1f); // TODO this doesnt make them float up a bit, right? It might... fine tune mby, idk.
	}

	public void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.Position += Vector3.up;
		player.Scale = Vector3.one;
	}
}

internal class AustralianConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 12f;
	public float MaxDuration { get; set; } = 36f;
	public float RarityMultiplier { get; set; } = 1f;
}
