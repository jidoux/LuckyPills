namespace LuckyPills.Effects;

internal sealed class TeleportToRandomPlayer(TeleportToRandomPlayerConfig config) : IPillEffect {
	// TODO continue testing this as its pretty iffy
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		foreach (Player currPlayer in Player.ReadyList) {
			if (currPlayer.IsInNonScpTeam() && currPlayer != player) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've been teleported to a random non-SCP";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		Player? randomPlayer = Player.ReadyList
			.Where(currPlayer => currPlayer.IsInNonScpTeam())
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		if (randomPlayer is null) {
			Logger.Warn("TeleportToRandomPlayer pill triggered when there is no player. Could be because the other/last player just died, or an error in the code.");
			return;
		}
		player.Position = randomPlayer.Position + Vector3.up; // not sure if the +1 is needed for this.. some other teleports sent you thru the floor.
	}
}

internal sealed class TeleportToRandomPlayerConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
