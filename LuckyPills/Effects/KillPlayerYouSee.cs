namespace LuckyPills.Effects;

internal sealed class KillPlayerYouSee : KillPlayerYouSeeConfig, IPillEffect, IDebugPickPills {
	// TODO this doesnt work for some reason, idkw hy.
	public new bool IsEnabled(Player player) {
		if (base.IsEnabled && Physics.Raycast(player.Camera.position, player.Camera.forward, out RaycastHit hit, maxDistance: 100f)) {
			Player? targetPlayer = Player.Get(hit.collider.gameObject);
			if (targetPlayer is not null && targetPlayer != player && targetPlayer.IsAlive) {
				return true;
			}
		}
		return false;
	}
	public string DisplayText => "You've killed anyone you're looking at";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		if (player.Camera is null) {
			Logger.Error("Player has no camera transform, somehow.");
			return;
		}

		if (Physics.Raycast(player.Camera.position, player.Camera.forward, out RaycastHit hit, maxDistance: 100f)) {
			Player? targetPlayer = Player.Get(hit.collider.gameObject);
			if (targetPlayer is not null && targetPlayer != player && targetPlayer.IsAlive) {
				targetPlayer.Damage(float.MaxValue, "Died from an intense stare");
				return; // If it succeeded, lets just return so I can log the failure case easier.
			}
		}
		Logger.Debug($"{this.GetType().Name} didn't actually kill anyone... maybe the player turned quickly away from someone?? Idk.");
	}
}

internal class KillPlayerYouSeeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
