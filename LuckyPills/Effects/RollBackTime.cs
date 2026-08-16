namespace LuckyPills.Effects;

/*
 TODO
1.) If you are dead 90 seconds ago, make it just ignore you 90 seconds later. I think this
is the more fun approach. My strategy for this rule is just ignoring dead ppl when initially snapshotting.
 */
internal sealed class RollBackTime : RollBackTimeConfig, IPillEffect, IDebugPickPills {
	// TODO make some fixed-size of 90 stack or something. Might be good to use a linked list for this, idk.
	// needs to evict oldest and only keep 90..
	private static readonly Stack<Vector3> _previousPositions = new Stack<Vector3>(90)

	public new bool IsEnabled(Player player) => base.IsEnabled;
	public string DisplayText => "You've rolled back time by 90 seconds";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		MEC.Timing.RunCoroutine(BuildPreviousPositions(player));
	}

	public void OnDisabled(Player player) {
		player.Scale = Vector3.one;
	}

	private static IEnumerator<float> BuildPreviousPositions(Player player) {
		while (true) {
			_previousPositions.Add(player.Position);
			yield return MEC.Timing.WaitForSeconds(0.25f);
		}
	}
}

internal class RollBackTimeConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
