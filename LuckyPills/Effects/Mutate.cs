using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed record Mutate : MutateConfig, IPillEffect {
	private readonly Dictionary<Player, RoleTypeId> _cachedRoles = [];

	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've been mutated for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(base.MinDuration, base.MaxDuration);
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		if (!_cachedRoles.ContainsKey(player)) {
			_cachedRoles.Add(player, player.Role);
		}
		player.DropAllItems();
		player.SetRole(RoleTypeId.Scp0492, RoleChangeReason.ItemUsage, RoleSpawnFlags.None);
	}

	public void OnDisabled(Player player) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		if (player.IsAlive && _cachedRoles.TryGetValue(player, out RoleTypeId role)) {
			player.SetRole(role, RoleChangeReason.ItemUsage, RoleSpawnFlags.None);
			_cachedRoles.Remove(player);
		}
	}
}

internal record MutateConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 5f;
	public float MaxDuration { get; set; } = 26f;
	public float RarityMultiplier { get; set; } = 1f;
}
