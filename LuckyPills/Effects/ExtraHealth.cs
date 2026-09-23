namespace LuckyPills.Effects;

internal sealed class ExtraHealth(ExtraHealthConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => player.Health < 99f && config.IsEnabled;
	public string DisplayText { get; } = "You've been permanently given extra health";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
		int healthAmountToGive = config.AmountOfHealthToGive;
		if (Random.value <= 0.01) {
			healthAmountToGive *= 2; // Why not give them a bit more.
		}
		player.MaxHealth += healthAmountToGive;
		player.Heal(player.MaxHealth);
	}
}

internal sealed class ExtraHealthConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
	public int AmountOfHealthToGive { get; set; } = 100;
}
