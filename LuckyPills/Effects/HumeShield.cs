namespace LuckyPills.Effects;

internal sealed class HumeShield(HumeShieldConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given some shield";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		if (player.MaxHumeShield > 1f) {
			player.MaxHumeShield += config.AmountOfShieldToGive;
		}
		else {
			player.MaxHumeShield = config.AmountOfShieldToGive;
			player.HumeShieldRegenCooldown = 15f;
			player.HumeShieldRegenRate = 12.5f;
			player.HumeShield = config.AmountOfShieldToGive;
		}
	}
}

internal sealed class HumeShieldConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public int AmountOfShieldToGive { get; set; } = 100;
}
