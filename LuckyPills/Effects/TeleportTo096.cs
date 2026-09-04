using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class TeleportTo096(TeleportTo096Config config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		foreach (Player currPlayer in Player.ReadyList) {
			if (currPlayer.Role == RoleTypeId.Scp096) {
				return true;
			}
		}
		return false;
	}

	public string DisplayText { get; } = "You've been teleported to SCP-096";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		foreach (Player currPlayer in Player.ReadyList) {
			if (currPlayer.Role == RoleTypeId.Scp096) {
				player.Position = currPlayer.Position + Vector3.up; // not sure if the +1 is needed for this.. some other teleports sent you thru the floor.
				return; // I want to early exit + log when we somehow cant meet this condition.
			}
		}
		Logger.Warn("TeleportTo096 pill triggered when there is no SCP-096. Could be because SCP-096 just died, or an error in the code.");
	}
}

internal sealed class TeleportTo096Config {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
