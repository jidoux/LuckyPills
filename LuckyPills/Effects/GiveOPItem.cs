using InventorySystem;
using InventorySystem.Items;

namespace LuckyPills.Effects;

internal sealed class GiveOPItem : GiveOPItemConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been given an OP item";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll | EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		ItemType[] itemPool = [
			ItemType.MicroHID,
			ItemType.ParticleDisruptor,
			ItemType.Jailbird,
			ItemType.GunCom45,
		];
		player.Inventory.ServerAddItem(itemPool[Random.Range(0, itemPool.Length)], ItemAddReason.AdminCommand);
	}
}

internal class GiveOPItemConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
