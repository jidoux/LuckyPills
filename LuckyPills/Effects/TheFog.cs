namespace LuckyPills.Effects;

internal sealed class TheFog(TheFogConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've created the fog";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		SpawnScp244(player.Position);
	}
}

internal sealed class TheFogConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
