using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryonePills(GiveEveryonePillsConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've given every player Painkillers";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		foreach (Player anyPlayerInMap in Player.ReadyList.Where(x => x.IsAlive || (config.GiveToScpsAlso && x.Team == Team.SCPs))) {
			if (anyPlayerInMap.Items.Count() >= 8) {
				Pickup.Create(ItemType.Painkillers, player.Position);
				if (anyPlayerInMap != player) {
					anyPlayerInMap.SendHint("You've been given Painkillers by someone else's Painkillers", duration: 4);
				}
			}
			else {
				if (anyPlayerInMap != player) {
					anyPlayerInMap.SendHint("You've been given Painkillers by someone else's Painkillers", duration: 4);
				}
				anyPlayerInMap.ForceEquip(ItemType.Painkillers);
			}
		}
	}
}

internal sealed class GiveEveryonePillsConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 40;
	public bool GiveToScpsAlso { get; set; } = true;
}
