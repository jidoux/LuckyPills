using PlayerRoles;

namespace LuckyPills.Effects;

internal record TeleportTo096 : PillEffect {

	protected override bool IsEnabled => (Player.List.FirstOrDefault(x => x.Role == RoleTypeId.Scp096) is not null && true); // TODO the 2nd thing here should be loaded from config imo
	protected override string DisplayText => "You've been teleported to SCP-096";

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Player? playerWhoIsScp096 = Player.List.FirstOrDefault(x => x.Role == RoleTypeId.Scp096);
		if (playerWhoIsScp096 is null) {
			Logger.Warn("TeleportTo096 pill triggered when there is no SCP-096. Could be because SCP-096 just died, or an error in the code.");
			return;
		}
		player.Position = playerWhoIsScp096.Position + UnityEngine.Vector3.up;
	}
}
