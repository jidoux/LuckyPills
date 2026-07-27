namespace LuckyPills.Effects;

internal sealed record Tantrum : TantrumConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "Oh...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		TantrumHazard.Spawn(position: player.Position, rotation: Quaternion.identity, scale: Vector3.one * base.TantrumSizeMultiplier);
		// TODO will this slow player down orr?
	}
}

internal record TantrumConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
	public float TantrumSizeMultiplier = 2f;
}
