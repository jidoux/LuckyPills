using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class TeleportToRandomPlayer : TeleportToRandomPlayerConfig, IPillEffect {
	// TODO continue testing this as its pretty iffy
	public new bool IsEnabled(Player player) =>
		Player.List.Any(
			currPlayer => IsPlayerInTeam(currPlayer, Team.ClassD, Team.Scientists, Team.FoundationForces, Team.ChaosInsurgency)
			&& currPlayer != player)
		&& base.IsEnabled;
	public string DisplayText => "You've been teleported to a random non-SCP";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Player? randomPlayer = Player.List
			.Where(currPlayer => IsPlayerInTeam(currPlayer, Team.ClassD, Team.Scientists, Team.FoundationForces, Team.ChaosInsurgency))
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		if (randomPlayer is null) {
			Logger.Warn("TeleportToRandomPlayer pill triggered when there is no player. Could be because the other/last player just died, or an error in the code.");
			return;
		}
		player.Position = randomPlayer.Position + Vector3.up; // not sure if the +1 is needed for this.. some other teleports sent you thru the floor.
	}
}

internal class TeleportToRandomPlayerConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
