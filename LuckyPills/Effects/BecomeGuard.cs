using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class BecomeGuard : BecomeGuardConfig, IPillEffect {
	// This will only be enabled if the round is less than 35 seconds long, there are 4 D class/scientist alive,
	// and less than 3 guards alive (the values are subject to change, the premise not so much).
	public new bool IsEnabled(Player player) {
		if (!base.IsEnabled) {
			return false;
		}
		if (Round.Duration.TotalSeconds > 35f) {
			return false;
		}
		byte eligibleCandidates = 0;
		foreach (Player currPlayer in Player.List) {
			if (eligibleCandidates < 4 && (currPlayer.Role == RoleTypeId.ClassD || currPlayer.Role == RoleTypeId.Scientist)) {
				eligibleCandidates++;
			}
		}
		return eligibleCandidates > 3;
	}
	public string DisplayText { get; } = "You and someone else have became Facility Guards";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Player guardBuddy = Player.List
			.Where(x => x != player && (x.Role == RoleTypeId.Scientist || x.Role == RoleTypeId.ClassD))
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		player.SetRole(RoleTypeId.FacilityGuard, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.All);
		if (guardBuddy is null) {
			Logger.Warn("Could not find a guard buddy for BecomeGuard effect :(");
		}
		else {
			guardBuddy.SetRole(RoleTypeId.FacilityGuard, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.All);
			// Avoiding any issues where stuff isn't properly initialized. 0.05f is arbitrary time and probably is too long.
			MEC.Timing.CallDelayed(0.05f, () => {
				guardBuddy.SendHint("Someone's Painkillers have made you a facility guard alongside them", duration: 4);
				guardBuddy.Position = player.Position;
			});
		}
	}
}

internal class BecomeGuardConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.45f;
}
