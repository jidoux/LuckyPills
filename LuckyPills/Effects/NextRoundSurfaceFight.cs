using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class NextRoundSurfaceFight(NextRoundSurfaceFightConfig config) : IPillEffect {
	private static bool _nextRoundSurfaceFight = false;
	private static bool _thisRoundSurfaceFight = false;
	private static int _teamDeterminer = 0;

	// The Player.List count will work for 2 player lobbies (at least in my testing there was a 3rd npc or something... idk)
	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundSurfaceFight && Player.ReadyList.Count() > 2 && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		IsSpecialEventHappeningNextRound = true;
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
			Elevator.LockAll(); // Obviously pivotal
			foreach (Player currPlayer in Player.GetAll()) {
				if (_teamDeterminer % 2 == 0) {
					currPlayer.SetRoleDelay(chaosRoles[Random.Range(0, chaosRoles.Length)], RoleChangeReason.RemoteAdmin, RoleSpawnFlags.UseSpawnpoint);
				}
				else {
					currPlayer.SetRoleDelay(mtfRoles[Random.Range(0, chaosRoles.Length)], RoleChangeReason.RemoteAdmin, RoleSpawnFlags.UseSpawnpoint);
				}
				_teamDeterminer++;
				currPlayer.ClearInventory();
				for (int i = 0; i < 7; i++) {
					currPlayer.AddItem(ItemType.Jailbird);
				}
				currPlayer.ForceEquip(ItemType.Jailbird);
				currPlayer.SendHint("Someone's Painkillers from last round has caused a Chaos vs MTF surface fight", duration: 5f);
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
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundSurfaceFightConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.25f;
}
