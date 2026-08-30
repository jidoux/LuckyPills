namespace LuckyPills.Effects;

internal sealed class HumeShield(HumeShieldConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given some shield";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		// TODO change to  be ArtificialHealth and give them REAL hume shield/AHP like the SCPS fully rechargable too.
		// experiment with player.CreateAhpProcess
		player.HumeShield += config.AmountOfShieldToGive;
	}
}

internal sealed class HumeShieldConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float AmountOfShieldToGive { get; set; } = 100f;
}
