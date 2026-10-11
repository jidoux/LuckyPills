namespace LuckyPills.Effects;

// TODO validate that this is a good effect since it seems genuinely horrible LOL
// Also TODO, this (and the inverted effect in general) wont work if the player can get other
// slowness effect, idk if super speed affects it. In general im concerned that other speed-manipulating
// effects remove this. But for now dont want to test around with that -_- so deal with it l8tr ok?
internal sealed class NextRoundTrippyRound(NextRoundTrippyRoundConfig config) : IPillEffect {
	private static bool _nextRoundTrippyRound = false;
	private static bool _thisRoundTrippyRound = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundTrippyRound && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundTrippyRound = true;
	}

	public static void NextRoundTrippyRoundBehavior(Player player) {
		LogCallIfDebug();
		if (_thisRoundTrippyRound) {
			LogCallIfDebug();
			player.SendHint("Someone's Painkillers from last round has triggered a trippy round", duration: 5f);
			// See InvertedMovement.cs for explanation on why this works
			player.EnableEffect<CustomPlayerEffects.Slowness>(intensity: 200, duration: 3600f, addDuration: true);
			player.Gravity = new Vector3(0f, -2f, -0f);
		}
	}

	public void OnRoundEnd() {
		if (_thisRoundTrippyRound) {
			_thisRoundTrippyRound = false;
			_nextRoundTrippyRound = false;
		}
		else if (_nextRoundTrippyRound) {
			_thisRoundTrippyRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundTrippyRoundConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 10;
}
