using InventorySystem;
using InventorySystem.Items;

namespace LuckyPills.Effects;

internal sealed class GiveOPItem : GiveOPItemConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given an OP item";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		ItemType[] itemPool = [
			ItemType.MicroHID,
			ItemType.ParticleDisruptor,
			ItemType.Jailbird,
			ItemType.GunCom45,
		];
		player.Inventory.ServerAddItem(itemPool[Random.Range(0, itemPool.Length)], ItemAddReason.AdminCommand);
		// TODO maybe make this force equip?
	}
}

internal class GiveOPItemConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
