#define DEBUGGING_SPECIFIC_PILL_EFFECTS_WITH_INTERFACE // comment this out when you dont want to only use pill effects deriving from IDebugPickPills

namespace LuckyPills;

/// <summary>
/// See use of this interface later on; used to pick what pills are active in debug mode.
/// </summary>
internal interface IDebugPickPills;

internal interface IPillEffect {
	public bool IsEnabled(Player player);
	public string DisplayText { get; }
	public virtual Duration PossibleDurationRangeInclusive => new(float.MaxValue, float.MaxValue); // Default to max duration; this is virtual due to interface segregation.
	/// <summary>
	/// 1 is the base priority, 0.1 would be 10 times rarer than base, etc.
	/// </summary>
	public float RarityMultiplier => 1f;
	public void OnEnabled(Player player, float duration);
	public virtual void OnDisabled(Player player) { } // Virtual rather than abstract due to interface segregation principle.
	public EffectCapabilities Capabilities { get; }
}

internal static class PillEffectOrchestrator {
#if DEBUGGING_SPECIFIC_PILL_EFFECTS_WITH_INTERFACE && DEBUG
	private static readonly IEnumerable<IPillEffect> _allPillEffects = SharedCode.GetAllPillEffects(useDebugPickPills: true);
#else
	private static readonly IEnumerable<IPillEffect> _allPillEffects = SharedCode.GetAllPillEffects();
#endif

	public static void RunRandom(Player player) {
		IPillEffect? selectedEffect = GetRandomPillEffect(player);
		if (selectedEffect is null) {
			Logger.Warn("There are no enabled effects to select.");
			return;
		}
		float duration = selectedEffect.PossibleDurationRangeInclusive.Random;
		selectedEffect.OnEnabled(player, duration);
		player.SendHint(selectedEffect.DisplayText.Replace("{duration}", ((int)Math.Floor(duration)).ToString()));
		#pragma warning disable S1244 // I figure this warning is pointless when comparing against Float.MaxValue as long as im not doing computations...
		if (duration != float.MaxValue) {
			MEC.Timing.CallDelayed(duration, () => selectedEffect.OnDisabled(player)); // Can sometimes just do nothing if OnDisabled isn't overriden.
		}
		#pragma warning restore S1244
	}

	private static IPillEffect? GetRandomPillEffect(Player player) {
		List<IPillEffect> allEnabledPillEffects = _allPillEffects.Where(x => x.IsEnabled(player)).ToList();
		if (allEnabledPillEffects.Count == 0) {
			return null; // Signal to the caller that some error occurred - think Result<T>
		}

		float totalWeight = allEnabledPillEffects.Sum(x => x.RarityMultiplier);
		float randomRoll = Random.Range(0f, totalWeight);
		float cumulativeWeightSum = 0f;
		foreach (IPillEffect pillEffect in allEnabledPillEffects) {
			cumulativeWeightSum += pillEffect.RarityMultiplier;
			if (randomRoll < cumulativeWeightSum) {
				return pillEffect;
			}
		}
		// I figure this is a reasonable fallback
		return allEnabledPillEffects[allEnabledPillEffects.Count - 1];
	}

	/// <summary>
	/// Generally this is used for certain effects to be involved with other effects.
	/// </summary>
	[Flags]
	public enum EffectCapabilities {
		None = 0,
		/// <summary>
		/// Used because there is 1 rare effect which gives you all vomit effects.
		/// </summary>
		VomitEffect = 1 << 0,
		/// <summary>
		/// Used because there is 1 rare effect which gives you "all effects".
		/// </summary>
		CandidateForGiveAll = 1 << 1,
		GoodEffect = 1 << 2,
	}

	internal readonly struct Duration(float Minimum, float Maximum) {
		public float Random => UnityEngine.Random.Range(Minimum, Maximum);
	}
}
