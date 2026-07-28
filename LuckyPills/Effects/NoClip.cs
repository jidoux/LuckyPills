namespace LuckyPills.Effects;

// this one is kinda absurd... sometimes its fine, but you can easily get hard-stuck...
internal sealed class NoClip : NoClipConfig, IPillEffect {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given Noclip for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None; // Yeah not touching this one ahahaha

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsNoclipEnabled = true;
	}

	public void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		player.IsNoclipEnabled = false; // TODO if player dies in noclip is this a problem? OR can player not die in noclip
	}
}

internal class NoClipConfig { // TODO fix this whole config values... bad defaults but fun values for now
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 20f;
	public float MaxDuration { get; set; } = 30f;
	public float RarityMultiplier { get; set; } = 0.5f;
}
