namespace LuckyPills.Effects;

internal sealed class NextRoundEveryPillIsBallVomit(NextRoundEveryPillIsBallVomitConfig config) : IPillEffect {
	// Doing this to ensure it pulls the up-to-date config values.
	private static readonly Lazy<BallVomit> _ballVomitInstance = new(() => {
		return new BallVomit(Plugin.Singleton.Config.BallVomit);
	});
	// Adding bomb vomit to this as well, why not ya know?
	private static readonly Lazy<BombVomit> _bombVomitInstance = new(() => {
		return new BombVomit(Plugin.Singleton.Config.BombVomit);
	});
	// I was told to give them god mode as well... maybe this is a bad idea idk
	private static readonly Lazy<God> _godModeInstance = new(() => {
		return new God(new GodConfig {
			MinDuration = 5f,
			MaxDuration = 6f,
		});
	});

	private static bool _nextRoundEveryPillIsBalls = false;
	private static bool _thisRoundEveryPillIsBalls = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundEveryPillIsBalls && _ballVomitInstance.Value.IsEnabled(player)
		&& _bombVomitInstance.Value.IsEnabled(player) && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundEveryPillIsBalls = true;
	}

	public static bool TryDoNextRoundBallVomitBehavior(Player player) {
		if (!_thisRoundEveryPillIsBalls) {
			return false;
		}
		if (Random.Range(0, 2) == 1) { // not sure the cleanest way to write 50% chances but this is 50% chance
			ActivateEffect(player, _ballVomitInstance.Value);
		}
		else {
			ActivateEffect(player, _bombVomitInstance.Value);
		}
		ActivateEffect(player, _godModeInstance.Value);
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
	public float RarityMultiplier { get; set; } = 0.1f; // Probably should be rarer than all the other "next round" ones.
}
