using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class ResurrectTeamMember : ResurrectTeamMemberConfig, IPillEffect, IDebugPickPills {
	// My approach: add to it when a player dies, but don't remove when they respawn. If they die again, remove
	// the old player's entry and add the new one. So we just have a list of players and their data when they last
	// died, and we access the list by filtering out living players.
	private static readonly Dictionary<Player, RoleAndTeamInfo> _killedPlayers = [];

	public new bool IsEnabled(Player player) {
		if (!base.IsEnabled) {
			return false;
		}
		foreach (KeyValuePair<Player, RoleAndTeamInfo> item in _killedPlayers) {
			if (!item.Key.IsAlive && item.Value.Team == player.Team) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've resurrected a fallen team member";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodEffect;

	public void OnEnabled(Player player, float duration) {
		KeyValuePair<Player, RoleAndTeamInfo> respawnPlayerInfo = _killedPlayers
			.Where(x => x.Value.Team == player.Team && !x.Key.IsAlive)
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		if (respawnPlayerInfo.Equals(default(KeyValuePair<Player, RoleAndTeamInfo>))) {
			Logger.Warn("ResurrectTeamMember called where there are no players to respawn. Possibly the player just died...?");
			return;
		}
		Player playerToRespawn = respawnPlayerInfo.Key;
		RoleTypeId roleToSetTo = respawnPlayerInfo.Value.Role;
		playerToRespawn.SetRole(roleToSetTo, RoleChangeReason.Revived, RoleSpawnFlags.AssignInventory);
		playerToRespawn.Position = player.Position;
		playerToRespawn.SendHint("You've been resurrected by someone's Painkillers");
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

internal class ResurrectTeamMemberConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
