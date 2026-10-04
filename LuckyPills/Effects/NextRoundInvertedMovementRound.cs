namespace LuckyPills.Effects;

// TODO validate that this is a good effect since it seems genuinely horrible LOL
// Also TODO, this (and the inverted effect in general) wont work if the player can get other
// slowness effect, idk if super speed affects it. In general im concerned that other speed-manipulating
// effects remove this. But for now dont want to test around with that -_- so deal with it l8tr ok?
internal sealed class NextRoundInvertedMovementRound(NextRoundInvertedMovementRoundConfig config) : IPillEffect {
	private static bool _nextRoundInvertedMovementRound = false;
	private static bool _thisRoundInvertedMovementRound = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundInvertedMovementRound && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundInvertedMovementRound = true;
	}

	public static void NextRoundInvertedMovementRoundBehavior(Player player) {
		if (_thisRoundInvertedMovementRound) {
			player.SendHint("Someone's Painkillers from last round has triggered an inverted movement round", duration: 5f);
			// Idk why this works, but yeah giving max slowness inverts movement??? Lol
			// 209 was too fast. 200 felt like the normal player speed... my understanding is that its some
			// overflow scenario.
			player.EnableEffect<CustomPlayerEffects.Slowness>(intensity: 200, duration: 3600f, addDuration: true);
		}
	}

	public void OnRoundEnd() {
		if (_thisRoundInvertedMovementRound) {
			_thisRoundInvertedMovementRound = false;
			_nextRoundInvertedMovementRound = false;
		}
		else if (_nextRoundInvertedMovementRound) {
			_thisRoundInvertedMovementRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundInvertedMovementRoundConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 20;
}
