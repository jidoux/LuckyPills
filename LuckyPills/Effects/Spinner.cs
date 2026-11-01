namespace LuckyPills.Effects;

internal sealed class Spinner(SpinnerConfig config) : IPillEffect {
	private static readonly HashSet<Player> _allSpinningPlayers = [];

	public bool IsEnabled(Player player) => config.IsEnabled && !_allSpinningPlayers.Contains(player);
	public string DisplayText { get; } = "You're gonna be spinning for {duration} seconds";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		if (_allSpinningPlayers.Add(player)) {
			Timing.RunCoroutine(SpinPlayer(player, duration, config.SpinsPerSecond));
		}
		else {
			Logger.Error("Error with Spinner: player is already in the _allSpinningPlayers set.");
		}
	}

	// Called when player dies or whatever, so its reasonable to do this even though its technically infinite.
	public void OnDisabled(Player player) {
		_allSpinningPlayers.Remove(player);
	}

	public void OnRoundEnd() {
		_allSpinningPlayers.Clear();
	}

	private static IEnumerator<float> SpinPlayer(Player player, int duration, int spinsPerSecond) {
		try {
			float delayTime = 1f / spinsPerSecond;
			Quaternion initialRotation = player.Rotation;

			float angle = 0f;
			float elapsed = 0f;

			do {
				if (!player.IsAlive) {
					yield break;
				}

				elapsed += delayTime;

				if (elapsed <= duration) {
					angle += 360f / spinsPerSecond;
				}
				else {
					angle += Mathf.DeltaAngle(angle, 0f) * 0.2f;
				}

				player.Rotation = initialRotation * Quaternion.AngleAxis(angle, Vector3.up);
				yield return Timing.WaitForSeconds(delayTime);
			} while (elapsed < duration || Mathf.Abs(Mathf.DeltaAngle(angle, 0f)) > 0.5f);
		}
		finally {
			_allSpinningPlayers.Remove(player);
		}
	}
}

internal sealed class SpinnerConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 15;
	public int MaxDuration { get; set; } = 30;
	public ushort SpinsPerSecond { get; set; } = 8; // :)
	public ushort RarityWeight { get; set; } = 80;
}
