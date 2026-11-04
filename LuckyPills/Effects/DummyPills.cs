using Mirror;
using NetworkManagerUtils.Dummies;
using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class DummyPills(DummyPillsConfig config) : IPillEffect, IDebugPickPills {
	private static readonly Dictionary<Player, DummyInfo> _dummies = [];

	public bool IsEnabled(Player player) => config.IsEnabled && !_dummies.ContainsKey(player);
	public string DisplayText { get; } = $"A dummy will eat a Painkiller behind you every {config.EatIntervalSeconds} seconds until one of you die";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll; // currently its permanent already

	public void OnEnabled(Player player, int duration) {
		if (_dummies.ContainsKey(player)) {
			Logger.Error($"Error with DummyPills: {player.Nickname} already has a dummy.");
			return;
		}

		ReferenceHub hub = DummyUtils.SpawnDummy();
		Player dummy = Player.Get(hub);
		dummy.SetRoleDelay(RoleTypeId.ClassD);
		// NOTE: I was getting some issues where, after killing a dummy and immediately respawning it, it was just
		// disappearing. So I store the LifeId which fixed the issue, so all operations I do on the dummy, I first check
		// to make sure the LifeId of the dummy (reference type) is the same as the (value type) life id. Not exactly sure
		// how, but it appeared as if DummyUtils.SpawnDummy caches some stuff or smth; seemed like my old routines had
		// the reference of the new dummy.
		int lifeId = dummy.LifeId;
		DummyInfo dummyInfo = new(dummy, lifeId);

		_dummies[player] = dummyInfo;
		Timing.RunCoroutine(FollowRoutine(player, dummyInfo));
		Timing.RunCoroutine(EatRoutine(player, dummyInfo, config.EatIntervalSeconds));
	}

	public void OnDisabled(Player player) => RemoveDummy(player);

	public void OnRoundEnd() {
		foreach (Player player in _dummies.Keys.ToArray()) {
			RemoveDummy(player);
		}
	}

	private static IEnumerator<float> EatRoutine(Player player, DummyInfo dummyInfo, int eatIntervalSeconds) {
		try {
			yield return Timing.WaitForOneFrame;
			yield return Timing.WaitForSeconds(eatIntervalSeconds);
			int dummyId = dummyInfo.LifeId;
			Player dummy = dummyInfo.Dummy;

			while (player.IsAlive && dummy.LifeId == dummyId && dummy.IsAlive && _dummies.ContainsKey(player)) {
				Item? item = dummy.ForceEquip(ItemType.Painkillers);
				yield return Timing.WaitForSeconds(1);
				if (item is not null && dummy.LifeId == dummyId) {
					dummy.RemoveItem(item);
				}
				if (dummy.LifeId == dummyId) {
					RunRandom(dummy);
				}
				yield return Timing.WaitForSeconds(eatIntervalSeconds - 1); // As the dummy holding the pill took up 1 second anyway.
			}
		}
		finally {
			if (dummyInfo.Dummy.LifeId == dummyInfo.LifeId) {
				RemoveDummy(player);
			}
		}
	}

	private static IEnumerator<float> FollowRoutine(Player player, DummyInfo dummyInfo) {
		try {
			yield return Timing.WaitForOneFrame;
			yield return Timing.WaitForSeconds(0.5f); // Just in case

			const float tick = 0.1f;
			const float followDistance = 2f;
			const float teleportDistance = 15f;
			const float followSpeed = 6f;

			int dummyId = dummyInfo.LifeId;
			Player dummy = dummyInfo.Dummy;

			if (dummyInfo.Dummy.LifeId == dummyId) {
				dummy.Position = player.Position + player.Rotation * Vector3.back * followDistance;
			}

			while (_dummies.ContainsKey(player) && player.IsAlive && dummy.LifeId == dummyId && dummy.IsAlive) {
				Vector3 toPlayer = player.Position - dummy.Position;
				float distance = toPlayer.magnitude;

				if (distance > teleportDistance && dummy.LifeId == dummyId) {
					dummy.Position = player.Position + player.Rotation * Vector3.back * followDistance;
				}
				else if (distance > followDistance && dummy.LifeId == dummyId) {
					float step = Math.Min(followSpeed * tick, distance - followDistance);
					dummy.Position += toPlayer / distance * step;
				}

				Vector3 flat = new(toPlayer.x, 0f, toPlayer.z);
				if (flat.sqrMagnitude > 0.01f && dummy.LifeId == dummyId) {
					dummy.Rotation = Quaternion.LookRotation(flat);
				}

				yield return Timing.WaitForSeconds(tick);
			}
		}
		finally {
			if (dummyInfo.Dummy.LifeId == dummyInfo.LifeId) {
				RemoveDummy(player);
			}
		}
	}

	private static void RemoveDummy(Player player) {
		if (player.IsDummy) { // In case the dummy died, cleaning everything up just to be safe.
			Player? playerWhoOwnedTheDummy = null;
			foreach (KeyValuePair<Player, DummyInfo> item in _dummies) {
				if (item.Value.Dummy == player) {
					playerWhoOwnedTheDummy = item.Key;
					break;
				}
			}
			if (playerWhoOwnedTheDummy is not null) {
				_dummies.Remove(playerWhoOwnedTheDummy);
				NetworkServer.Destroy(player.GameObject);
			}
		}
		if (_dummies.Remove(player, out DummyInfo dummy)) {
			NetworkServer.Destroy(dummy.Dummy.GameObject);
		}
	}

	private readonly record struct DummyInfo(Player Dummy, int LifeId);
}

internal sealed class DummyPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public int EatIntervalSeconds { get; set; } = 5;
	public ushort RarityWeight { get; set; } = 70;
}
