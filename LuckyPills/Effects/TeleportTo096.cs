using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class TeleportTo096 : TeleportTo096Config, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) {
		if (!base.IsEnabled) {
			return false;
		}
		foreach (Player currPlayer in Player.List) {
			if (currPlayer.Role == RoleTypeId.Scp096) {
				return true;
			}
		}
		return false;
	}

	public string DisplayText { get; } = "You've been teleported to SCP-096";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player currPlayer in Player.List) {
			if (currPlayer.Role == RoleTypeId.Scp096) {
				player.Position = currPlayer.Position + Vector3.up; // not sure if the +1 is needed for this.. some other teleports sent you thru the floor.
				return; // I want to early exit + log when we somehow cant meet this condition.
			}
		}
		Logger.Warn("TeleportTo096 pill triggered when there is no SCP-096. Could be because SCP-096 just died, or an error in the code.");
	}
}

internal class TeleportTo096Config {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
