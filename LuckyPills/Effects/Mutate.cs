using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class Mutate : MutateConfig, IPillEffect {
	private readonly Dictionary<Player, RoleTypeId> _cachedRoles = [];

	public new bool IsEnabled(Player player) {
		if (!base.IsEnabled) {
			return false;
		}
		byte counter = 0;
		foreach (Player currPlayer in Player.List) {
			if (currPlayer.IsInNonScpTeam()) {
				counter++;
			}
			if (counter > 1) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText { get; } = "You've been mutated for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		if (!_cachedRoles.ContainsKey(player)) {
			_cachedRoles.Add(player, player.Role);
		}
		player.DropAllItems();
		player.SetRole(RoleTypeId.Scp0492, RoleChangeReason.ItemUsage, RoleSpawnFlags.None);
	}

	public void OnDisabled(Player player) {
		if (player.IsAlive && _cachedRoles.TryGetValue(player, out RoleTypeId role)) {
			player.SetRole(role, RoleChangeReason.ItemUsage, RoleSpawnFlags.None);
			_cachedRoles.Remove(player);
		}
	}
}

internal class MutateConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 9f;
	public float MaxDuration { get; set; } = 36f;
	public float RarityMultiplier { get; set; } = 0.8f;
}
