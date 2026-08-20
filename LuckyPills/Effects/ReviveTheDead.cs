using PlayerRoles;

namespace LuckyPills.Effects;

// TODO it was suggested that they should be revived with guns. Yeah idk about that one lol...
// TODO test this.. I imagine it will work, but idk man
internal sealed class ReviveTheDead : ReviveTheDeadConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => IsPlayerInTeam(player, Team.ClassD, Team.ChaosInsurgency, Team.FoundationForces, Team.Scientists)
		&& Player.List.Count(x => !x.IsAlive) >= base.NumDeadPlayersNecessaryToBeEnabled && base.IsEnabled;
	public string DisplayText => "You've revived the dead";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		IEnumerable<Player> playersToRevive = Player.List
			.Where(currPlayer => !currPlayer.IsAlive && IsPlayerInTeam(currPlayer, Team.ClassD, Team.ChaosInsurgency, Team.FoundationForces, Team.Scientists))
			.OrderBy(_ => Random.value)
			.Take(base.MaxDeadPlayersToRevive);
		foreach (Player playerToRevive in playersToRevive) {
			playerToRevive.SetRole(player.Role, RoleChangeReason.Revived, RoleSpawnFlags.AssignInventory);
			playerToRevive.Position = player.Position;
			playerToRevive.SendHint("Someone's Painkillers has revived the dead");
		}
	}
}

internal class ReviveTheDeadConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f; // TODO lower this imo to 0.2 or something idk
	public int NumDeadPlayersNecessaryToBeEnabled = 3;
	public int MaxDeadPlayersToRevive = 999;
}
