using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks : GiveEveryoneAksConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText { get; } = "You've given every player an AK";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player anyPlayerInMap in Player.List.Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			anyPlayerInMap.SendHint("You've been given an AK by someone else's Painkillers");
			if (anyPlayerInMap.Items.Count() >= 8) {
				anyPlayerInMap.SpawnItemBelow(ItemType.GunAK);
			}
			else {
				anyPlayerInMap.ForceEquip(ItemType.GunAK);
			}
		}
	}
}

internal class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.8f;
}
