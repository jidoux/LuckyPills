using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks(GiveEveryoneAksConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've given every player an AK";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		foreach (Player anyPlayerInMap in GetAllPlayers().Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			if (anyPlayerInMap.IsInventoryFull) {
				// Yeah I have no clue how to spawn a loaded gun on the ground :)
				Pickup.Create(ItemType.GunAK, anyPlayerInMap.Position);
				anyPlayerInMap.AddAmmo(ItemType.Ammo762x39, 30);
			}
			else {
				if (anyPlayerInMap != player) {
					anyPlayerInMap.SendHint("You've been given an AK by someone else's Painkillers", duration: 4);
				}
				anyPlayerInMap.ForceEquip(ItemType.GunAK);
			}
		}
	}
}

internal sealed class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 80;
}
