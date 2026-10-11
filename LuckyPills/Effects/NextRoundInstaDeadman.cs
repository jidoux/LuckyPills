namespace LuckyPills.Effects;

internal sealed class NextRoundInstaDeadman(NextRoundInstaDeadmanConfig config) : IPillEffect, IDebugPickPills {
	private static bool _instaDeadmanNextRound = false;
	private static bool _instaDeadmanThisRound = false;
	/// <summary>
	/// There is a bug where, for some reason, it doesn't get activated if triggered a second time. Restarting the server seems to fix the
	/// issue. I don't really see it as a big issue to just have it only activate once per-server uptime.
	/// </summary>
	private static bool _deadmanAlreadyRanAtSomePoint = false; // TODO this is a bad solution but I don't feel like fixing it, no clue how to either.

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_instaDeadmanNextRound && !_deadmanAlreadyRanAtSomePoint && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
		_instaDeadmanNextRound = true;
	}

	public static void NextRoundInstaDeadmanBehavior() {
		LogCallIfDebug();
		if (_instaDeadmanThisRound) {
			LogCallIfDebug();
			Timing.CallDelayed(1.5f, () => {
				DeadmanSwitch.InitiateProtocol(); // TODO unsure if this is best way to do it, seems problematic due to bug I mentioned in other TODOs on this page.
			});
		}
	}

	public void OnRoundEnd() {
		if (_instaDeadmanThisRound) {
			_instaDeadmanThisRound = false;
			_instaDeadmanNextRound = false;
			_deadmanAlreadyRanAtSomePoint = true; // TODO TESTING either delete or re activate code if works idk
		}
		else if (_instaDeadmanNextRound) {
			_instaDeadmanThisRound = true;
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundInstaDeadmanConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 5;
}
