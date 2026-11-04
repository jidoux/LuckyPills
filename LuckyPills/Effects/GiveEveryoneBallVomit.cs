namespace LuckyPills.Effects;

internal sealed class GiveEveryoneBallVomit(GiveEveryoneBallVomitConfig config) : IPillEffect {
	public bool IsEnabled(Player player) => Plugin.Singleton.Config.BallVomit.IsEnabled
		&& Round.Duration > TimeSpan.FromMinutes(config.RoundDurationMinutesUntilThisCanBeEnabled)
		&& config.IsEnabled;
	public string DisplayText { get; } = "You've given everyone ball vomit";
	public Duration PossibleDurationRangeInclusive => new(config.MinDuration, config.MaxDuration);
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		BallVomit ballVomitInstance = new(Plugin.Singleton.Config.BallVomit);

		foreach (Player anyPlayerInMap in GetAllPlayers().Where(x => x.IsAlive)) {
			anyPlayerInMap.SendHint("You've been given ball vomit by someone else's Painkillers", duration: 4);
			EnablePillEffect(ballVomitInstance, anyPlayerInMap, duration);
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
