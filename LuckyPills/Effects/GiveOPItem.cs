namespace LuckyPills.Effects;

internal sealed class GiveOPItem : GiveOPItemConfig, IPillEffect {
	private static readonly ItemType[] _itemPool = [
			ItemType.MicroHID,
			ItemType.ParticleDisruptor,
			ItemType.Jailbird,
			ItemType.GunCom45,
		];

	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "You've been given an OP item";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		player.ForceEquip(_itemPool[Random.Range(0, _itemPool.Length)]);
	}
}

internal class GiveOPItemConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.9f;
}
