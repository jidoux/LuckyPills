using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class BeAHero(BeAHeroConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled || player.Team == Team.SCPs || player.Items.Any()) {
			return false;
		}
		foreach (Player currPlayer in GetAllPlayers()) {
			if (currPlayer.Team == Team.SCPs && currPlayer.Role != RoleTypeId.Scp079) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've been chosen to be a hero";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		Player? randomScp = GetAllPlayers()
			.Where(x => x.Team == Team.SCPs && x.Role != RoleTypeId.Scp079)
			.OrderBy(x => Random.value)
			.FirstOrDefault();
		if (randomScp is null) {
			Logger.Warn("BeAHero couldn't find an SCP, somehow. I guess the scp literally just died...?");
			return;
		}
		Item? lastItem = null;
		for (int i = 0; i < 8; i++) {
			lastItem = player.AddItem(ItemType.Painkillers, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
		player.CurrentItem = lastItem;
		player.Position = randomScp.Position + Vector3.up;
	}
}

internal sealed class BeAHeroConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 40;
}
