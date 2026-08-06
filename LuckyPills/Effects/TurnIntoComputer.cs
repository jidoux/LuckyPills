using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;

namespace LuckyPills.Effects;

internal sealed class TurnIntoComputer : TurnIntoComputerConfig, IPillEffect, IDebugPickPills {
	// TODO test this as its kinda hard to test.
	public new bool IsEnabled(Player player) =>
		Player.List.Count(x => x.Role != RoleTypeId.ClassD && x.Role != RoleTypeId.Scientist && x != player) > 2 // At least 3 class D/scientists, not counting current pill popper
		&& !Player.List.Any(x => x.Role == RoleTypeId.Scp079) // We don't want there to already be a 079
		&& Map.Generators.Count(x => x.Engaged) >= 3 // If the generators are engaged to where SCP-079 should be dead.
		&& base.IsEnabled;
	public string DisplayText => "You've turned into SCP-079";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		if (player.IsAlive) {
			player.SetRole(RoleTypeId.Scp079, RoleChangeReason.ItemUsage, RoleSpawnFlags.All);
			// TODO set to max level
		}
	}
}

internal class TurnIntoComputerConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
