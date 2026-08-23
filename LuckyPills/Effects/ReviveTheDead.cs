using PlayerRoles;

namespace LuckyPills.Effects;

// TODO it was suggested that they should be revived with guns. Yeah idk about that one lol...
// TODO test this.. I imagine it will work, but idk man
internal sealed class ReviveTheDead : ReviveTheDeadConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) {
		if (!base.IsEnabled || !player.IsInNonScpTeam()) {
			return false;
		}
		ushort counter = 0;
		foreach (Player currPlayer in Player.List) {
			if (currPlayer.IsAlive) {
				counter++;
			}
			if (counter == base.NumDeadPlayersNecessaryToBeEnabled) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've revived the dead";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		IEnumerable<Player> playersToRevive = Player.List
			.Where(currPlayer => !currPlayer.IsAlive && currPlayer.IsInNonScpTeam())
			.OrderBy(_ => Random.value)
			.Take(base.MaxDeadPlayersToRevive);
		foreach (Player playerToRevive in playersToRevive) {
			playerToRevive.SetRole(player.Role, RoleChangeReason.Revived, RoleSpawnFlags.AssignInventory);
			playerToRevive.Position = player.Position;
			playerToRevive.SendHint("Someone's Painkillers have revived the dead", duration: 4);
		}
	}
}

internal class ReviveTheDeadConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.2f;
	public int NumDeadPlayersNecessaryToBeEnabled { get; set; } = 3;
	public int MaxDeadPlayersToRevive { get; set; } = 999;
}
