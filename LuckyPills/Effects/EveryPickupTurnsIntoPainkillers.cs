namespace LuckyPills.Effects;

internal sealed class EveryPickupTurnsIntoPainkillers : EveryPickupTurnsIntoPainkillersConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "Every item you pick up until the end of the game will turn into painkillers";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		GlobalVariables.PlayersWhoCanOnlyPickUpPillsForTheRestOfTheGame.Add(player);
	}

	// This will only get called by various event handlers.
	public void OnDisabled(Player player) {
		// Looks like Player doesn't override Equals, so using the reference hub to determine what player was previously added.
		// TODO validate that this works in every scenario, cuz idk.
		GlobalVariables.PlayersWhoCanOnlyPickUpPillsForTheRestOfTheGame.RemoveAll(x => x.ReferenceHub.Equals(player.ReferenceHub));
	}
}

internal class EveryPickupTurnsIntoPainkillersConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
