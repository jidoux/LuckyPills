using Mirror;
using NetworkManagerUtils.Dummies;
using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class DummyPills(DummyPillsConfig config) : IPillEffect, IDebugPickPills {
	private static readonly Dictionary<Player, Player> _dummies = [];

	public bool IsEnabled(Player player) => config.IsEnabled && !_dummies.ContainsKey(player);
	public string DisplayText { get; } = $"A dummy will eat a Painkiller behind you every {config.EatIntervalSeconds} seconds until one of you die";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.GoodAsPermanent;

	public void OnEnabled(Player player, int duration) {
		if (_dummies.ContainsKey(player)) {
			Logger.Error($"Error with DummyPills: {player.Nickname} already has a dummy.");
			return;
		}

		ReferenceHub hub = DummyUtils.SpawnDummy();
		Player dummy = Player.Get(hub);
		dummy.SetRoleDelay(RoleTypeId.ClassD);

		_dummies[player] = dummy;
		Timing.RunCoroutine(FollowRoutine(player, dummy));
		Timing.RunCoroutine(EatRoutine(player, dummy, duration, config.EatIntervalSeconds));
	}

	public void OnDisabled(Player player) => RemoveDummy(player);

	public void OnRoundEnd() {
		foreach (Player player in _dummies.Keys.ToArray()) {
			RemoveDummy(player);
		}
	}

	private static IEnumerator<float> EatRoutine(Player player, Player dummy, int duration, int eatIntervalSeconds) {
		try {
			yield return Timing.WaitForOneFrame;
			yield return Timing.WaitForSeconds(eatIntervalSeconds);

			dummy.Position = player.Position + player.Rotation * Vector3.back * 2f;


			while (player.IsAlive && dummy.IsAlive && _dummies.ContainsKey(player)) {
				Item? item = dummy.ForceEquip(ItemType.Painkillers);
				yield return Timing.WaitForSeconds(1);
				if (item is not null) {
					dummy.RemoveItem(item);
				}
				RunRandom(dummy);
				yield return Timing.WaitForSeconds(eatIntervalSeconds - 1); // As the dummy holding the pill took up 1 second anyway.
			}
		}
		finally {
			RemoveDummy(player);
		}
	}

	private static IEnumerator<float> FollowRoutine(Player player, Player dummy) {
		try {
			yield return Timing.WaitForOneFrame;
			yield return Timing.WaitForSeconds(1f); // Just in case

			const float tick = 0.1f;
			const float followDistance = 2f;
			const float teleportDistance = 15f;
			const float followSpeed = 6f;

			dummy.Position = player.Position + player.Rotation * Vector3.back * followDistance;

			while (_dummies.ContainsKey(player) && player.IsAlive && dummy.IsAlive) {
				Vector3 toPlayer = player.Position - dummy.Position;
				float distance = toPlayer.magnitude;

				if (distance > teleportDistance) {
					dummy.Position = player.Position + player.Rotation * Vector3.back * followDistance;
				}
				else if (distance > followDistance) {
					float step = Math.Min(followSpeed * tick, distance - followDistance);
					dummy.Position += toPlayer / distance * step;
				}

				Vector3 flat = new(toPlayer.x, 0f, toPlayer.z);
				if (flat.sqrMagnitude > 0.01f) {
					dummy.Rotation = Quaternion.LookRotation(flat);
				}

				yield return Timing.WaitForSeconds(tick);
			}
		}
		finally {
			RemoveDummy(player);
		}
	}

	private static void RemoveDummy(Player player) {
		if (player.IsDummy) { // In case the dummy died, cleaning everything up just to be safe.
			Player? playerWhoOwnedTheDummy = null;
			foreach (KeyValuePair<Player, Player> item in _dummies) {
				if (item.Value == player) {
					playerWhoOwnedTheDummy = item.Key;
					break;
				}
			}
			if (playerWhoOwnedTheDummy is not null) {
				_dummies.Remove(playerWhoOwnedTheDummy);
				NetworkServer.Destroy(player.GameObject);
			}
		}
		if (_dummies.Remove(player, out Player? dummy)) {
			NetworkServer.Destroy(dummy.GameObject);
		}
	}
}

internal sealed class DummyPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public int EatIntervalSeconds { get; set; } = 5;
	public ushort RarityWeight { get; set; } = 40;
}
