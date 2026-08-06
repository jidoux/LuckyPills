using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class TeleportTo096 : TeleportTo096Config, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => Player.List.FirstOrDefault(x => x.Role == RoleTypeId.Scp096) is not null && base.IsEnabled;
	public string DisplayText => "You've been teleported to SCP-096";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Player? playerWhoIsScp096 = Player.List.FirstOrDefault(x => x.Role == RoleTypeId.Scp096);
		if (playerWhoIsScp096 is null) {
			Logger.Warn("TeleportTo096 pill triggered when there is no SCP-096. Could be because SCP-096 just died, or an error in the code.");
			return;
		}
		player.Position = playerWhoIsScp096.Position + Vector3.up; // not sure if the +1 is needed for this.. some other teleports sent you thru the floor.
	}
}

internal class TeleportTo096Config {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
