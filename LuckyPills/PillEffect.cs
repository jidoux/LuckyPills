namespace LuckyPills;

internal abstract record PillEffect {
	protected abstract bool IsEnabled { get; }

	protected abstract string DisplayText { get; }

	protected virtual Duration Duration { get; } = new(10, 20); // Arbitrary default; this is virutal due to interface segregation.

	protected abstract void OnEnabled(Player player, int duration);

	protected virtual void OnDisabled(Player player) { } // Virtual rather than abstract due to interface segregation principle.

	public static void RunRandom(Player player) {		
		PillEffect? selectedEffect = GetRandomPillEffect();
		if (selectedEffect is null) {
			Logger.Warn("There are no enabled effects to select.");
			return;
		}
		int duration = selectedEffect.Duration?.Get() ?? 0;
		selectedEffect.OnEnabled(player, duration);
		player.SendHint(selectedEffect.DisplayText.Replace("{duration}", duration.ToString()));
		MEC.Timing.CallDelayed(duration, () => selectedEffect.OnDisabled(player)); // Can sometimes just do nothing if OnDisabled isn't overriden.
	}

	// TODO - is this really a good approach, using reflection like this? Idk, but at least it enables dynamic checking of
	// pill conditions.
	public static PillEffect? GetRandomPillEffect() {
		// Generic code to get all classes which inherit from a given interface / abstract class.
		IEnumerable<PillEffect> allPillEffects = typeof(PillEffect).Assembly.GetTypes()
				.Where(x => typeof(PillEffect).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
				.Select(x => (PillEffect)Activator.CreateInstance(x)!);
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
