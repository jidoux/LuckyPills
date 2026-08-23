namespace LuckyPills.Effects;

internal sealed class NextRoundEveryPillIsBallVomit : NextRoundEveryPillIsBallVomitConfig, IPillEffect {
	private static readonly BallVomit _ballVomitInstance = new();
	private static readonly BombVomit _bombVomitInstance = new(); // Adding bomb vomit to this as well, why not ya know?

	private static bool _nextRoundEveryPillIsBalls = false;
	private static bool _thisRoundEveryPillIsBalls = false;

	public new bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundEveryPillIsBalls && _ballVomitInstance.IsEnabled(player)
		&& _bombVomitInstance.IsEnabled(player) && base.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public new float RarityMultiplier => base.RarityMultiplier;
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
			ActivateEffect(player, _ballVomitInstance);
		}
		else {
			ActivateEffect(player, _bombVomitInstance);
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

internal class NextRoundEveryPillIsBallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.25f;
}
