namespace LuckyPills.Effects;

internal sealed class EveryPickupTurnsIntoPainkillers : EveryPickupTurnsIntoPainkillersConfig, IPillEffect, IDebugPickPills {
	private static readonly HashSet<Player> _playersWhoCanOnlyPickUpPillsForTheRestOfTheGame = [];

	public new bool IsEnabled(Player player) =>
		!_playersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Contains(player) && base.IsEnabled;
	public string DisplayText => "Every item you pick up until the end of the game will turn into painkillers";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

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

internal class EveryPickupTurnsIntoPainkillersConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
