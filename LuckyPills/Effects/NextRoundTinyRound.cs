namespace LuckyPills.Effects;

internal sealed class NextRoundTinyRound(NextRoundTinyRoundConfig config) : IPillEffect {
	private static bool _nextRoundTinyRound = false;
	private static bool _thisRoundTinyRound = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundTinyRound && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundTinyRound = true;
	}

	public static void NextRoundTinyRoundBehavior(Player player) {
		if (_thisRoundTinyRound) {
			player.SendHint("Someone's Painkillers from last round has triggered a tiny round", duration: 5f);
			player.Scale = new Vector3(0.2f, 0.2f, 0.2f);
		}
	}

	public void OnRoundEnd() {
		if (_thisRoundTinyRound) {
			_thisRoundTinyRound = false;
			_nextRoundTinyRound = false;
		}
		else if (_nextRoundTinyRound) {
			_thisRoundTinyRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundTinyRoundConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 20;
}
