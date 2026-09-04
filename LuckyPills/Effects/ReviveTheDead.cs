using PlayerRoles;

namespace LuckyPills.Effects;

// TODO it was suggested that they should be revived with guns. Yeah idk about that one lol...
// TODO test this.. I imagine it will work, but idk man
internal sealed class ReviveTheDead(ReviveTheDeadConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled || !player.IsInNonScpTeam()) {
			return false;
		}
		ushort counter = 0;
		foreach (Player currPlayer in Player.ReadyList) {
			if (!currPlayer.IsAlive) {
				counter++;
			}
			if (counter == config.NumDeadPlayersNecessaryToBeEnabled) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've revived the dead";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IEnumerable<Player> playersToRevive = Player.ReadyList
			.Where(currPlayer => !currPlayer.IsAlive)
			.OrderBy(_ => Random.value)
			.Take(config.MaxDeadPlayersToRevive);
		foreach (Player playerToRevive in playersToRevive) {
			playerToRevive.SetRoleDelay(player.Role, RoleChangeReason.Revived, RoleSpawnFlags.AssignInventory);
			playerToRevive.Position = player.Position;
			// Delaying since some "next round" things can also show hint text upon respawn,
			// but this is significantly more important to display.
			MEC.Timing.CallDelayed(0.15f, () => {
				playerToRevive.SendHint("Someone's Painkillers have revived the dead", duration: 5);
			});
		}
	}
}

internal sealed class ReviveTheDeadConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 30;
	public int NumDeadPlayersNecessaryToBeEnabled { get; set; } = 3;
	public int MaxDeadPlayersToRevive { get; set; } = 999;
}
