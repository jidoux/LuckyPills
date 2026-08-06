namespace LuckyPills.Effects;

internal sealed class ActivateNuke : ActivateNukeConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've activate the nuclear warhead";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		// TODO implement this
	}
}

internal class ActivateNukeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
