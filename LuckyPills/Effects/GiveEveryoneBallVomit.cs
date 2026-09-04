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
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		foreach (Player anyPlayerInMap in Player.ReadyList.Where(x => x.IsAlive)) {
			anyPlayerInMap.SendHint("You've been given ball vomit by someone else's Painkillers", duration: 4);
			EnablePillEffect(_ballVomitInstance.Value, anyPlayerInMap, duration);
		}
	}
}

internal sealed class GiveEveryoneBallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public int MinDuration { get; set; } = 10;
	public int MaxDuration { get; set; } = 20;
	public ushort RarityWeight { get; set; } = 20;
	public int RoundDurationMinutesUntilThisCanBeEnabled = 10;
}
