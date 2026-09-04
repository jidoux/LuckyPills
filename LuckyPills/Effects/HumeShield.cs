namespace LuckyPills.Effects;

internal sealed class HumeShield(HumeShieldConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given some shield";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
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
	public ushort RarityWeight { get; set; } = 100;
	public int AmountOfShieldToGive { get; set; } = 100;
}
