using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;

namespace LuckyPills.Effects;

internal sealed class TurnIntoComputer(TurnIntoComputerConfig config) : IPillEffect, IDebugPickPills {
	// TODO I am moderately unsure if I want to swap one of the SCPs to a D-Class when this gets activated, like idk man.
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		// At least 3 class D/scientists
		byte relevantPlayers = 0;
		foreach (Player currPlayer in Player.ReadyList) {
			if (currPlayer.Role == RoleTypeId.Scp079) {
				return false;
			}
			if (relevantPlayers <= 3 && (currPlayer.Role == RoleTypeId.ClassD || currPlayer.Role == RoleTypeId.Scientist)) {
				relevantPlayers++;
			}
		}
		byte engagedCount = 0;
		foreach (Generator generator in Map.Generators) {
			if (generator.Engaged) {
				engagedCount++;
			}
		}
		return engagedCount < 3 && relevantPlayers > 3;
	}

	public string DisplayText { get; } = "You've turned into SCP-079";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		player.SetRoleDelay(RoleTypeId.Scp079, RoleChangeReason.ItemUsage, RoleSpawnFlags.All);
		if (TryGetScp079TierManager(player.RoleBase, out Scp079TierManager? tierManager)) {
			SetScp079ExpLevel(tierManager, 10000);
		}
	}
}

internal sealed class TurnIntoComputerConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 20;
}
