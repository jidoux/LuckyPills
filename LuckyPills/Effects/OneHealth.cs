namespace LuckyPills.Effects;

internal sealed class OneHealth(OneHealthConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => player.MaxHealth > 99f && config.IsEnabled;
	public string DisplayText { get; } = "Your health has been permanently lowered";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.MaxHealth = 1;
	}

	public void OnDisabled(Player player) {
		player.MaxHealth = 100f;
	}
}

internal sealed class OneHealthConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 80;
}
