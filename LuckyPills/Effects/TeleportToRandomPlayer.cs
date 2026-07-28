namespace LuckyPills.Effects;

internal sealed class TeleportToRandomPlayer : TeleportToRandomPlayerConfig, IPillEffect {
	// TODO this sometimes doesnt work
	// NOTE: For some reason there's always a role with type of none, in the game.. at least during my testing, idk why.
	public new bool IsEnabled => 
		Player.List.Count(
			x => !x.Role.ToString().StartsWith("scp", StringComparison.OrdinalIgnoreCase)
				&& x.Role != PlayerRoles.RoleTypeId.None
				&& x.Role != PlayerRoles.RoleTypeId.Spectator
		) > 1 // We need count > 1 as there needs to be at least 2 valid players (player taking the pill, and someone else).
		&& base.IsEnabled;
	public string DisplayText => "You've been teleported to a random non-SCP";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		foreach (var item in Player.List) {
			// TODO - delete this when its confirmed that this works... I have to assume there's some case I don't know about, so.
			Logger.Debug("Teleport to random player called with this player in the list: " + item.Role.ToString());
		}
		Player? randomPlayer = Player.List
			.Where(x => x != player && !x.Role.ToString().StartsWith("scp", StringComparison.OrdinalIgnoreCase) && x.Role != PlayerRoles.RoleTypeId.None)
			.OrderBy(x => Random.value) // TODO check this cuz i dont know if its working correctly or not
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
