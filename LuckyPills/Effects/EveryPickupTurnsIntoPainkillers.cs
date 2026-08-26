namespace LuckyPills.Effects;

// TODO this broke after I got it one game, reset, and someone else got it next game. Validate that this is fixed ok?
internal sealed class EveryPickupTurnsIntoPainkillers(EveryPickupTurnsIntoPainkillersConfig config) : IPillEffect {
	private static readonly HashSet<Player> _playersWhoCanOnlyPickUpPillsForTheRestOfTheGame = [];

	public bool IsEnabled(Player player) =>
		!_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Contains(player) && config.IsEnabled;
	public string DisplayText { get; } = "Every item you pick up until the end of the game will turn into painkillers";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Add(player);
	}

	// This will only get called by various event handlers.
	public void OnDisabled(Player player) {
		_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Remove(player);
	}

	public static bool ShouldPickupTurnIntoPills(Player player) => _playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Contains(player);

	public void OnRoundEnd() {
		_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Clear();
	}
}

internal sealed class EveryPickupTurnsIntoPainkillersConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
