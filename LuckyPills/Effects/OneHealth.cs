namespace LuckyPills.Effects;

internal sealed class OneHealth : OneHealthConfig, IPillEffect {
	public new bool IsEnabled(Player player) => player.MaxHealth > 99f && player.MaxHealth < 101f && base.IsEnabled;
	public string DisplayText { get; } = "Your health has been permanently lowered";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		player.MaxHealth = 1;
	}

	public void OnDisabled(Player player) {
		player.MaxHealth = 100f;
	}
}

internal class OneHealthConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.8f;
}
