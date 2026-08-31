namespace LuckyPills.Effects;

internal sealed class GiveOPItem(GiveOPItemConfig config) : IPillEffect {
	private static readonly ItemType[] _itemPool = [
			ItemType.MicroHID,
			ItemType.ParticleDisruptor,
			ItemType.Jailbird,
			ItemType.GunCom45,
		];

	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've been given an OP item";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.ForceEquip(_itemPool.RandomItem());
	}
}

internal sealed class GiveOPItemConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.9f;
}
