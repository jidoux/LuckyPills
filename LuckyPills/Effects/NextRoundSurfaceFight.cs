using PlayerRoles;

namespace LuckyPills.Effects;

// TODO test this... also TODO disable the elevators, either put facility in blown up state or something idk man
internal sealed class NextRoundSurfaceFight : NextRoundSurfaceFightConfig, IPillEffect, IDebugPickPills {
	private static bool _nextRoundSurfaceFight = false;
	private static bool _thisRoundSurfaceFight = false;
	private static int _teamDeterminer = 0;

	public new bool IsEnabled(Player player) => !GlobalVariables.IsSpecialEventHappeningNextRound &&
		!_nextRoundSurfaceFight && Player.List.Count > 3 && base.IsEnabled;
	public string DisplayText => "Something special will happen next round...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		GlobalVariables.IsSpecialEventHappeningNextRound = true;
		_nextRoundSurfaceFight = true;
	}

	public static void NextRoundSurfaceFightBehavior() {
		RoleTypeId[] chaosRoles = [
			RoleTypeId.ChaosConscript,
			RoleTypeId.ChaosMarauder,
			RoleTypeId.ChaosRepressor,
			RoleTypeId.ChaosRifleman,
		];

		RoleTypeId[] mtfRoles = [
			RoleTypeId.NtfCaptain,
			RoleTypeId.NtfPrivate,
			RoleTypeId.NtfSergeant,
			RoleTypeId.NtfSpecialist,
		];

		if (_thisRoundSurfaceFight) {
			foreach (Elevator item in Map.Elevators) {
				item.DynamicAdminLock = true;
			} // TODO p sure this AINT GONA DO ANYHTING AHAHHAHAHA
			foreach (Player currPlayer in Player.GetAll()) {
				if (_teamDeterminer % 2 == 0) {
					currPlayer.SetRole(chaosRoles[Random.Range(0, chaosRoles.Length)], RoleChangeReason.RemoteAdmin, RoleSpawnFlags.UseSpawnpoint);
				}
				else {
					currPlayer.SetRole(mtfRoles[Random.Range(0, chaosRoles.Length)], RoleChangeReason.RemoteAdmin, RoleSpawnFlags.UseSpawnpoint);
				}
				_teamDeterminer++;
				currPlayer.ClearInventory();
				for (int i = 0; i < 7; i++) {
					currPlayer.AddItem(ItemType.Jailbird);
				}
				currPlayer.ForceEquip(ItemType.Jailbird);
				currPlayer.SendHint("Someone's Painkillers from last round has caused a Chaos vs MTF surface fight");
			}
		}
	}

	public void OnRoundEnd() {
		if (_thisRoundSurfaceFight) {
			_thisRoundSurfaceFight = false;
			_nextRoundSurfaceFight = false;
		}
		else if (_nextRoundSurfaceFight) {
			_thisRoundSurfaceFight = true;
			_teamDeterminer = 0;
			GlobalVariables.IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal class NextRoundSurfaceFightConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
}
