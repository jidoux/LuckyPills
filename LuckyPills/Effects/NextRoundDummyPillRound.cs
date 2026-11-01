namespace LuckyPills.Effects;

internal sealed class NextRoundDummyPillRound(NextRoundDummyPillRoundConfig config) : IPillEffect {
	private static bool _nextRoundDummyPillRound = false;
	private static bool _thisRoundDummyPillRound = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_nextRoundDummyPillRound && config.IsEnabled && Plugin.Singleton.Config.DummyPills.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_nextRoundDummyPillRound = true;
	}

	public static bool ShouldSpawnADummy() {
		// Technically this checks if the effect is enabled since _thisRoundDummyPillRound is only true if its enabled.
		return _thisRoundDummyPillRound && Random.Range(1, 3) == 2;
	}

	public void OnRoundEnd() {
		if (_thisRoundDummyPillRound) {
			_thisRoundDummyPillRound = false;
			_nextRoundDummyPillRound = false;
		}
		else if (_nextRoundDummyPillRound) {
			_thisRoundDummyPillRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundDummyPillRoundConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 250;// TODO CHANGEME
}
