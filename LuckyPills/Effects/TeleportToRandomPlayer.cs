namespace LuckyPills.Effects;

internal record TeleportToRandomPlayer : PillEffect, IDebugPickPills {

	protected override bool IsEnabled => (Player.List.Where(x => !x.Role.ToString().StartsWith("scp", StringComparison.OrdinalIgnoreCase)).ToList().Count > 1 && true); // TODO the 2nd thing here should be loaded from config imo
	protected override string DisplayText => "You've been teleported to a random non-SCP";

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Player? randomPlayer = Player.List
			.Where(x => x != player && !x.Role.ToString().StartsWith("scp", StringComparison.OrdinalIgnoreCase))
			.OrderBy(x => UnityEngine.Random.value)
			.FirstOrDefault();
		if (randomPlayer is null) {
			Logger.Warn("TeleportToRandomPlayer pill triggered when there is no player. Could be because the other/last player just died, or an error in the code.");
			return;
		}
		player.Position = randomPlayer.Position + UnityEngine.Vector3.up;
	}
}
