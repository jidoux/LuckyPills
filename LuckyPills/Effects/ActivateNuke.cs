namespace LuckyPills.Effects;

internal sealed class ActivateNuke : ActivateNukeConfig, IPillEffect {
	// Arbitrary choice to make it only active if nothing for nuke was activated... makes it more exciting when it happens.
	public new bool IsEnabled(Player player) => !Warhead.LeverStatus && !Warhead.IsAuthorized && base.IsEnabled;
	public string DisplayText => "You've activated the nuclear warhead";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Warhead.LeverStatus = true;
		Warhead.IsAuthorized = true;
		Warhead.Start(isAutomatic: false, suppressSubtitles: false, activator: player);

	}
}

internal class ActivateNukeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
}
