namespace LuckyPills.Effects;

internal sealed class SwapPositions(SwapPositionsConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) {
		if (!config.IsEnabled) {
			return false;
		}
		byte counter = 0;
		foreach (Player currPlayer in Player.ReadyList) {
			if (currPlayer.IsAlive && currPlayer.Role != PlayerRoles.RoleTypeId.Scp079) {
				counter++;
			}
			if (counter > 1) {
				return true;
			}
		}
		return false;
	}

	public string DisplayText { get; } = "You've swapped positions with another random player";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		Player? randomPlayer = Player.ReadyList
			.Where(x => x.IsAlive && x != player && x.Role != PlayerRoles.RoleTypeId.Scp079)
			.OrderBy(_ => Random.value)
			.FirstOrDefault();
		if (randomPlayer is null) {
			Logger.Warn("SwapPositions pill triggered when there is no player. Could be because the other/last player just died, or an error in the code.");
			return;
		}
		(randomPlayer.Position, player.Position) = (player.Position, randomPlayer.Position);
		(randomPlayer.Rotation, player.Rotation) = (player.Rotation, randomPlayer.Rotation);
		(randomPlayer.LookRotation, player.LookRotation) = (player.LookRotation, randomPlayer.LookRotation);
		randomPlayer.SendHint("Painkillers have swapped your position with another player", duration: 4.5f); // TODO validate this shows as someone alleged it didnt show
	}
}

internal sealed class SwapPositionsConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 80;
}
