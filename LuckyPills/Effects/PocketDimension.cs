namespace LuckyPills.Effects;

internal sealed class PocketDimension : PocketDimensionConfig, IPillEffect {
	public new bool IsEnabled(Player player) => base.IsEnabled;
	// No DisplayText here, I display it manually.
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		// When this effect is enabled it checks if pocket dimension's room identifier can be found, then teleports the player to it's position.
		// I was previously finding the room manually, which had some freaky issue where players could just have a permanent black screen, idk why.
		if (Random.value > base.OddsFromZeroToOneToSendEveryPlayerThere) {
			player.SendHint("You've been sent to the pocket dimension");
			player.EnableEffect<CustomPlayerEffects.PocketCorroding>();
		}
		else {
			foreach (Player currPlayer in Player.List) {
				if (currPlayer.IsAlive) {
					currPlayer.EnableEffect<CustomPlayerEffects.PocketCorroding>();
					currPlayer.SendHint("Someone's Painkillers have sent everyone to the Pocket Dimension", duration: 4);
				}
			}
		}
	}
}

internal class PocketDimensionConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.7f; // TODO check this value again, idk
	public float OddsFromZeroToOneToSendEveryPlayerThere = 0.1f;
}
