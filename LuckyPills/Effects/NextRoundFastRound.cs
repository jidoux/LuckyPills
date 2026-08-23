namespace LuckyPills.Effects;

internal sealed class NextRoundFastRound : NextRoundFastRoundConfig, IPillEffect {
	private static bool _nextRoundFastRound = false;
	private static bool _thisRoundFastRound = false;

	public new bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundFastRound && base.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundFastRound = true;
	}

	public static void NextRoundFastRoundBehavior(Player player) {
		if (_thisRoundFastRound) {
			player.SendHint("Someone's Painkillers from last round has triggered a fast round", duration: 5f);
			player.EnableEffect<CustomPlayerEffects.MovementBoost>(intensity: 50, duration: 3600f, addDuration: true);
		}
	}

	public void OnRoundEnd() {
		if (_thisRoundFastRound) {
			_thisRoundFastRound = false;
			_nextRoundFastRound = false;
		}
		else if (_nextRoundFastRound) {
			_thisRoundFastRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal class NextRoundFastRoundConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.25f;
}
