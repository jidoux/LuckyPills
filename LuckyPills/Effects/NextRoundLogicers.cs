using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class NextRoundLogicers(NextRoundLogicersConfig config) : IPillEffect {
	private static bool _giveDClassLogicersNextRound = false;
	private static bool _givingLogicersThisRound = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_giveDClassLogicersNextRound && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_giveDClassLogicersNextRound = true;
	}

	public static void NextRoundLogicersBehavior(Player player) {
		if (_givingLogicersThisRound && player.Role == RoleTypeId.ClassD) {
			player.SendHint("Someone's Painkillers from last round has given you this Logicer", duration: 5f);
			player.ForceEquip(ItemType.GunLogicer);
		}
	}

	public void OnRoundEnd() {
		if (_givingLogicersThisRound) {
			_givingLogicersThisRound = false;
			_giveDClassLogicersNextRound = false;
		}
		else if (_giveDClassLogicersNextRound) {
			_givingLogicersThisRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundLogicersConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 25;
}
