namespace LuckyPills.Effects;

internal sealed class WallHacks : WallHacksConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've been given wall hacks for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		// I have no clue what Scp1344Detected is... I gave it to a player and noticed nothing visually from my alt account.
		player.EnableEffect<CustomPlayerEffects.Scp1344>(intensity: 1, duration: duration, addDuration: true);
	}
}

internal class WallHacksConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 60f;
	public float MaxDuration { get; set; } = 240f;
	public float RarityMultiplier { get; set; } = 1f;
}
