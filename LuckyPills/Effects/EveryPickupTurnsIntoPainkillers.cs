namespace LuckyPills.Effects;

// TODO this broke after I got it one game, reset, and someone else got it next game. Validate that this is fixed ok?
internal sealed class EveryPickupTurnsIntoPainkillers(EveryPickupTurnsIntoPainkillersConfig config) : IPillEffect {
	private static readonly HashSet<Player> _playersWhoCanOnlyPickUpPillsForTheRestOfTheGame = [];

	public bool IsEnabled(Player player) =>
		!_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Contains(player) && config.IsEnabled;
	public string DisplayText { get; } = "Every item you pick up until the end of the game will turn into painkillers";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Add(player);
	}

	// NOTE: I decided that this should NOT call OnDisabled, and instead it will actually be active until the
	// rest of the game. However if it did call OnDisabled it would just remove the player from the HashSet.

	public static bool ShouldPickupTurnIntoPills(Player player) => _playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Contains(player);

	public void OnRoundEnd() {
		_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Clear();
	}
}

internal sealed class EveryPickupTurnsIntoPainkillersConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 100;
}
