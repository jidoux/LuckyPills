namespace LuckyPills.Effects;

internal sealed class HumeShield : HumeShieldConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given some shield";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.HumeShield += base.AmountOfShieldToGive;
	}
}

internal class HumeShieldConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float AmountOfShieldToGive { get; set; } = 100f;
}
