using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks(GiveEveryoneAksConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've given every player an AK";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player anyPlayerInMap in Player.ReadyList.Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			anyPlayerInMap.SendHint("You've been given an AK by someone else's Painkillers", duration: 4);
			if (anyPlayerInMap.Items.Count() >= 8) {
				anyPlayerInMap.SpawnItemBelow(ItemType.GunAK);
			}
			else {
				anyPlayerInMap.ForceEquip(ItemType.GunAK);
			}
		}
	}
}

internal sealed class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.8f;
}
