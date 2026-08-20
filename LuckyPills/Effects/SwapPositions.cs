namespace LuckyPills.Effects;

internal sealed class SwapPositions : SwapPositionsConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => Player.List.Count(x => x.IsAlive && x.Role != PlayerRoles.RoleTypeId.Scp079) > 1 && base.IsEnabled;
	public string DisplayText => "You've swapped positions with another random player";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Player? randomPlayer = Player.List
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
		randomPlayer.SendHint("Painkillers have swapped your position with another player"); // TODO validate this shows as someone alleged it didnt show
	}
}

internal class SwapPositionsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.7f;
}
