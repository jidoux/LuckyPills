namespace LuckyPills.Effects;

internal sealed class ExtraHealth(ExtraHealthConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => player.Health < 101f && config.IsEnabled;
	public string DisplayText { get; } = "You've been permanently given extra health";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
		int healthAmountToGive = config.AmountOfHealthToGive;
		// Why not give them a bit more, if they get lucky.
		if (Random.value <= 0.05) {
			healthAmountToGive *= 2;
			if (Random.value <= 0.05) {
				healthAmountToGive *= 2;
				if (Random.value <= 0.05) {
					healthAmountToGive *= 2;
					if (Random.value <= 0.04) {
						healthAmountToGive *= 2;
						if (Random.value <= 0.03) {
							healthAmountToGive *= 2;
							if (Random.value <= 0.02) {
								healthAmountToGive *= 2;
								if (Random.value <= 0.01) {
									healthAmountToGive *= 2;
								}
							}
						}
					}
				}
			}
		}
		player.MaxHealth += healthAmountToGive;
		player.Heal(player.MaxHealth);
	}
}

internal sealed class ExtraHealthConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 90;
	public int AmountOfHealthToGive { get; set; } = 100;
}
