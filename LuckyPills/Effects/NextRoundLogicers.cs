using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class NextRoundLogicers : NextRoundLogicersConfig, IPillEffect, IDebugPickPills {
	private static bool _giveDClassLogicersNextRound = false;
	private static bool _givingLogicersThisRound = false;

	public new bool IsEnabled(Player player) => !GlobalVariables.IsSpecialEventHappeningNextRound
		&& !_giveDClassLogicersNextRound && Player.List.Any(x => x.Role == RoleTypeId.ClassD) && base.IsEnabled;
	public string DisplayText => "Something special will happen next round..."; // TODO determine if this is preferable to no message at all, mby try both idk
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		GlobalVariables.IsSpecialEventHappeningNextRound = true;
		_giveDClassLogicersNextRound = true;
	}

	public static void NextRoundLogicersBehavior(Player player) {
		if (_givingLogicersThisRound && player.Role == RoleTypeId.ClassD) {
			player.SendHint("Someone's Painkillers from last round has given you this Logicer");
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
			GlobalVariables.IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal class NextRoundLogicersConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
}
