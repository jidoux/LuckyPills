//#define DEBUGGING_SPECIFIC_PILL_EFFECTS_WITH_INTERFACE // comment this out when you dont want to only use pill effects deriving from IDebugPickPills

namespace LuckyPills;

internal interface IDebugPickPills;

internal abstract record PillEffect {
	// TODO - idk if I need to make this a Func, or if properties can get evaluated at runtime evne though the class was instantiated once.
	// I really need to get all derived classes once with the reflection and then compute the IsEnabled for every pill... I think thats the move.
	protected abstract bool IsEnabled { get; }

	protected abstract string DisplayText { get; }

	protected virtual Duration PossibleDurationRangeInclusive { get; } = new(10f, 20f); // Arbitrary default; this is virutal due to interface segregation.

	protected abstract void OnEnabled(Player player, float duration);

	protected virtual void OnDisabled(Player player) { } // Virtual rather than abstract due to interface segregation principle.

	public static void RunRandom(Player player) {		
		PillEffect? selectedEffect = GetRandomPillEffect();
		if (selectedEffect is null) {
			Logger.Warn("There are no enabled effects to select.");
			return;
		}
		float duration = selectedEffect.PossibleDurationRangeInclusive.Random;
		selectedEffect.OnEnabled(player, duration);
		player.SendHint(selectedEffect.DisplayText.Replace("{duration}", ((int)Math.Floor(duration)).ToString()));
		MEC.Timing.CallDelayed(duration, () => selectedEffect.OnDisabled(player)); // Can sometimes just do nothing if OnDisabled isn't overriden.
	}

	// TODO - is this really a good approach, using reflection like this? Idk, but at least it enables dynamic checking of
	// pill conditions. Its necessary since some of these classes can be enabled or disabled at any given moment based on some conditions.
	private static PillEffect? GetRandomPillEffect() {
		IEnumerable<PillEffect> allPillEffects = Enumerable.Empty<PillEffect>();
		// Generic code to get all classes which inherit from a given interface / abstract class.
#if DEBUGGING_SPECIFIC_PILL_EFFECTS_WITH_INTERFACE && DEBUG
		allPillEffects = typeof(PillEffect).Assembly.GetTypes()
				.Where(x => typeof(IDebugPickPills).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(x => (PillEffect)Activator.CreateInstance(x)!);
#else
			allPillEffects = typeof(PillEffect).Assembly.GetTypes()
					.Where(x => typeof(PillEffect).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
					.Select(x => (PillEffect)Activator.CreateInstance(x)!);
#endif
		// Now lets get the pill effect to use.
		List<PillEffect> allEnabledPillEffects = allPillEffects.Where(x => x.IsEnabled).ToList();
		if (allEnabledPillEffects.Count == 0) {
			return null;
		}
		int randomPillToUse = UnityEngine.Random.Range(0, allEnabledPillEffects.Count);
		PillEffect selectedEffect = allEnabledPillEffects[randomPillToUse];
		return selectedEffect;
	}
}
