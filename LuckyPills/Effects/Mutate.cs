using PlayerRoles;

namespace LuckyPills.Effects;

internal record Mutate : PillEffect {
	private readonly Dictionary<Player, RoleTypeId> _cachedRoles = [];

	protected override bool IsEnabled { get; } = true;
	protected override string DisplayText { get; } = "You've been mutated for {duration} seconds";
	protected override Duration PossibleDurationRangeInclusive { get; } = new(5f, 26f);

	protected override void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		if (!_cachedRoles.ContainsKey(player)) {
			_cachedRoles.Add(player, player.Role);
		}
		player.DropAllItems();
		player.SetRole(RoleTypeId.Scp0492, RoleChangeReason.ItemUsage, RoleSpawnFlags.None);
	}

	protected override void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		if (player.IsAlive && _cachedRoles.TryGetValue(player, out RoleTypeId role)) {
			player.SetRole(role, RoleChangeReason.ItemUsage, RoleSpawnFlags.None);
			_cachedRoles.Remove(player);
		}
	}
}
