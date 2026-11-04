namespace LuckyPills.Effects;

internal sealed class NextRoundEveryPillIsBallVomit(NextRoundEveryPillIsBallVomitConfig config) : IPillEffect {
	private static bool _nextRoundEveryPillIsBalls = false;
	private static bool _thisRoundEveryPillIsBalls = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundEveryPillIsBalls && Plugin.Singleton.Config.BallVomit.IsEnabled
		&& Plugin.Singleton.Config.BombVomit.IsEnabled && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundEveryPillIsBalls = true;
	}

	public static bool TryDoNextRoundBallVomitBehavior(Player player) {
		BallVomit ballVomitInstance = new(Plugin.Singleton.Config.BallVomit);
		BombVomit bombVomitInstance = new(Plugin.Singleton.Config.BombVomit);
		God godModeInstance = new(new GodConfig {
			MinDuration = 5,
			MaxDuration = 6,
		});

		if (!_thisRoundEveryPillIsBalls) {
			return false;
		}
		LogCallIfDebug();
		ActivateEffect(player, godModeInstance, showPillMessage: false);
		if (Random.Range(0, 2) == 1) { // not sure the cleanest way to write 50% chances but this is 50% chance
			ActivateEffect(player, ballVomitInstance);
		}
		else {
			ActivateEffect(player, bombVomitInstance);
		}
		return true;
	}

	public void OnRoundEnd() {
		if (_thisRoundEveryPillIsBalls) {
			_thisRoundEveryPillIsBalls = false;
			_nextRoundEveryPillIsBalls = false;
		}
		else if (_nextRoundEveryPillIsBalls) {
			_thisRoundEveryPillIsBalls = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundEveryPillIsBallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 3; // Probably should be rarer than all the other "next round" ones.
}
