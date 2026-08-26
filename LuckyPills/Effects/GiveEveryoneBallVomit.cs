namespace LuckyPills.Effects;

internal sealed class GiveEveryoneBallVomit(GiveEveryoneBallVomitConfig config) : IPillEffect {
	private static readonly Lazy<BallVomit> _ballVomitInstance = new(() => {
		return new BallVomit(Plugin.Singleton.Config.BallVomit); // Doing this to ensure it pulls the up-to-date config value.
	});

	public bool IsEnabled(Player player) => _ballVomitInstance.Value.IsEnabled(player)
		&& Round.Duration > TimeSpan.FromMinutes(config.RoundDurationMinutesUntilThisCanBeEnabled)
		&& config.IsEnabled;
	public string DisplayText { get; } = "You've given everyone ball vomit";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		foreach (Player anyPlayerInMap in Player.ReadyList.Where(x => x.IsAlive)) {
			anyPlayerInMap.SendHint("You've been given ball vomit by someone else's Painkillers", duration: 4);
			EnablePillEffect(_ballVomitInstance.Value, anyPlayerInMap, duration);
		}
	}
}

internal sealed class GiveEveryoneBallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float MinDuration { get; set; } = 10f;
	public float MaxDuration { get; set; } = 20f;
	public float RarityMultiplier { get; set; } = 0.2f;
	public int RoundDurationMinutesUntilThisCanBeEnabled = 10;
}
