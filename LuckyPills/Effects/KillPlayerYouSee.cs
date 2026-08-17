using System.Diagnostics.CodeAnalysis;

namespace LuckyPills.Effects;

internal sealed class KillPlayerYouSee : KillPlayerYouSeeConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => base.IsEnabled && TryGetLookedAtPlayer(player, out Player? _);
	public string DisplayText => "You've killed whoever you're looking at";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		if (player.Camera is null) {
			Logger.Error("Player has no camera transform, somehow.");
			return;
		}

		if (TryGetLookedAtPlayer(player, out Player? targetPlayer)) {
			targetPlayer.Damage(float.MaxValue, "Smitten by Painkillers");
			return; // If it succeeded, lets just return so I can log the failure case every time.
		}
		Logger.Info($"{this.GetType().Name} didn't actually kill anyone... maybe the player turned quickly away from someone?? Idk.");
	}

	private static bool TryGetLookedAtPlayer(Player player, [NotNullWhen(true)] out Player? target) {
		target = null;
		if (player.Camera is null) {
			return false;
		}
		Vector3 origin = player.Camera.position + player.Camera.forward * 0.5f;
		bool didHit = Physics.Raycast(origin, player.Camera.forward, out RaycastHit hit, maxDistance: 100f, layerMask: ~0, queryTriggerInteraction: QueryTriggerInteraction.Collide);

		if (!didHit) {
			return false;
		}
		Player? hitPlayer = Player.Get(hit.collider.gameObject);
		hitPlayer ??= hit.collider.GetComponentInParent<ReferenceHub>() is { } hub ? Player.Get(hub) : null;

		if (hitPlayer is null || hitPlayer == player || !hitPlayer.IsAlive) {
			return false;
		}
		target = hitPlayer;
		return true;
	}
}

internal class KillPlayerYouSeeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
