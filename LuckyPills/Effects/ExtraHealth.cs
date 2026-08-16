namespace LuckyPills.Effects;

internal sealed class ExtraHealth : ExtraHealthConfig, IPillEffect {
	public new bool IsEnabled(Player player) => player.Health < 99f && base.IsEnabled;
	public string DisplayText => "You've been permanently given extra health";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.MaxHealth += base.AmountOfHealthToGive;
		player.Heal(player.MaxHealth);
	}
}

internal class ExtraHealthConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float AmountOfHealthToGive { get; set; } = 100f;
}
