namespace LuckyPills.Effects;

internal sealed class PocketDimension(PocketDimensionConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	// No DisplayText here, I display it manually.
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		// When this effect is enabled it checks if pocket dimension's room identifier can be found, then teleports the player to it's position.
		// I was previously finding the room manually, which had some freaky issue where players could just have a permanent black screen, idk why.
		if (Random.value > config.OddsFromZeroToOneToSendEveryPlayerThere) {
			player.SendHint("You've been sent to the pocket dimension");
			player.EnableEffect<CustomPlayerEffects.PocketCorroding>();
		}
		else {
			foreach (Player currPlayer in Player.ReadyList) {
				if (currPlayer.IsAlive && currPlayer.Role != PlayerRoles.RoleTypeId.Scp079) {
					currPlayer.EnableEffect<CustomPlayerEffects.PocketCorroding>();
					currPlayer.SendHint("Someone's Painkillers have sent everyone to the Pocket Dimension", duration: 4);
				}
			}
		}
	}
}

internal sealed class PocketDimensionConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.9f;
	public float OddsFromZeroToOneToSendEveryPlayerThere = 0.2f;
}
