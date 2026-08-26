using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class BecomeGuard(BecomeGuardConfig config) : IPillEffect {
	// This will only be enabled if the round is less than 35 seconds long, there are 4 D class/scientist
	// alive (the values are subject to change, the premise not so much).
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		if (Round.Duration.TotalSeconds > 35f) {
			return false;
		}
		byte eligibleCandidates = 0;
		foreach (Player currPlayer in Player.ReadyList) {
			if (eligibleCandidates < 4 && (currPlayer.Role == RoleTypeId.ClassD || currPlayer.Role == RoleTypeId.Scientist)) {
				eligibleCandidates++;
			}
		}
		return eligibleCandidates > 3;
	}
	public string DisplayText { get; } = "You and someone else have became Facility Guards";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Player guardBuddy = Player.ReadyList
			.Where(x => x != player && (x.Role == RoleTypeId.Scientist || x.Role == RoleTypeId.ClassD))
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		player.SetRoleDelay(RoleTypeId.FacilityGuard, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.All);
		if (guardBuddy is null) {
			Logger.Warn("Could not find a guard buddy for BecomeGuard effect :(");
		}
		else {
			guardBuddy.SetRoleDelay(RoleTypeId.FacilityGuard, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.All);
			guardBuddy.SendHint("Someone's Painkillers have made you a facility guard alongside them", duration: 4);
			guardBuddy.Position = player.Position;
		}
	}
}

internal sealed class BecomeGuardConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.45f;
}
