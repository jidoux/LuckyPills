using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class ResurrectTeamMember(ResurrectTeamMemberConfig config) : IPillEffect, IDebugPickPills {
	// My approach: add to it when a player dies, but don't remove when they respawn. If they die again, remove
	// the old player's entry and add the new one. So we just have a list of players and their data when they last
	// died, and we access the list by filtering out living players.
	private static readonly Dictionary<Player, RoleAndTeamInfo> _killedPlayers = [];

	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		foreach (KeyValuePair<Player, RoleAndTeamInfo> item in _killedPlayers) {
			// I noticed that leaving the server triggers OnPlayerDying, and either way if someone dies and then leaves
			// the server, they should no longer be eligible for resurrection.
			bool playerStillInServer = false;
			foreach (Player currPlayer in Player.ReadyList) {
				if (currPlayer == item.Key) {
					playerStillInServer = true;
					break;
				}
			}
			// TODO still need to test this, I believe?
			if (!playerStillInServer) {
				_killedPlayers.Remove(item.Key);
				break;
			}
			if (!item.Key.IsAlive && item.Value.Team == player.Team && item.Key != player) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've resurrected a fallen team member";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, int duration) {
		KeyValuePair<Player, RoleAndTeamInfo> respawnPlayerInfo = _killedPlayers
			.Where(x => x.Value.Team == player.Team && !x.Key.IsAlive)
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		if (respawnPlayerInfo.Equals(default(KeyValuePair<Player, RoleAndTeamInfo>))) {
			Logger.Warn("ResurrectTeamMember called where there are no players to respawn. Possibly the player just left...?");
			return;
		}
		Player playerToRespawn = respawnPlayerInfo.Key;
		RoleTypeId roleToSetTo = respawnPlayerInfo.Value.Role;
		playerToRespawn.SetRoleDelay(roleToSetTo, RoleChangeReason.Revived, RoleSpawnFlags.AssignInventory);
		playerToRespawn.Position = player.Position;
		// Delaying since some "next round" things can also show hint text upon respawn,
		// but this is significantly more important to display.
		MEC.Timing.CallDelayed(0.15f, () => {
			playerToRespawn.SendHint("You've been resurrected by someone's Painkillers", duration: 5);
		});
	}

	public void OnRoundEnd() {
		_killedPlayers.Clear();
	}

	public static void PlayerDied(Player player) {
		_killedPlayers.Remove(player);
		_killedPlayers.Add(player, new RoleAndTeamInfo(player.Role, player.Team));
	}

	private readonly record struct RoleAndTeamInfo(RoleTypeId Role, Team Team);
}

internal sealed class ResurrectTeamMemberConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
