
using System.Reflection;

namespace LuckyPills;

/// <summary>
/// See use of this interface later on; used to pick what pills are active in debug mode.
/// </summary>
internal interface IDebugPickPills;

/// <summary>
/// The contract of all pill effects. All virtual fields are virtual due to interface segregation.
/// </summary>
internal interface IPillEffect {
	public bool IsEnabled(Player player);
	public virtual string DisplayText => "";
	public virtual Duration PossibleDurationRangeInclusive => new(float.MaxValue, float.MaxValue); // Default to max duration.
	/// <summary>
	/// 1 is the base priority, 0.1 would be 10 times rarer than base, etc.
	/// </summary>
	public float RarityMultiplier => 1f;
	public void OnEnabled(Player player, float duration);
	public virtual void OnDisabled(Player player) { }
	public virtual void OnRoundEnd() { /*Fairly rare for effects to have this, but its a generic cleanup thing which is sometimes just defensive.*/ }
	public EffectCapabilities Capabilities { get; }
}

internal static class PillEffectOrchestrator {
	/// <summary>
	/// This is global for performance reasons. A better data type for it would be IReadOnlyCollection, but using an array for perf.
	/// I'm also keeping this here due to static initialization order with _enabledEffects.
	/// </summary>
	public static IPillEffect[] AllPillEffects { get; private set; } = PillEffectPopulator.GetAllPillEffects();

	/// <summary>
	/// This is populated of all enabled effects everytime a Painkiller is used. i want to initialize its size to the
	/// possible max to avoid overhead later on.
	/// </summary>
	private static readonly IPillEffect[] _enabledEffects = new IPillEffect[AllPillEffects.Length];
	public static bool IsSpecialEventHappeningNextRound { get; set; } = false;

	/// <summary>
	/// This gets called right when the plugin is ready/config loads. This is how I both cache all pill effects at startup
	/// and have each pill effect instance read the config value.
	/// </summary>
	public static void SetupPillEffects() {
		AllPillEffects = PillEffectPopulator.GetAllPillEffects();
	}

	public static void RunRandom(Player player) {
		IPillEffect? selectedEffect = GetRandomPillEffect(player);
		LogCallIfDebug("RunRandom; effect: " + selectedEffect?.GetType()?.Name ?? "ITS NULL IDK");
		if (selectedEffect is null) {
			Logger.Warn("There are no enabled effects to select.");
			return;
		}
		ActivateEffect(player, selectedEffect);
	}

	private static IPillEffect? GetRandomPillEffect(Player player) {
		float totalWeight = 0f;
		int totalEnabledPills = 0;
		foreach (IPillEffect effect in AllPillEffects) {
			if (effect.IsEnabled(player)) {
				totalWeight += effect.RarityMultiplier;
				_enabledEffects[totalEnabledPills] = effect;
				totalEnabledPills++;
			}
		}
		if (totalEnabledPills == 0) {
			return null; // Signal to the caller that some error occurred - think Result<T>
		}
		float randomRoll = Random.Range(0f, totalWeight);
		float cumulativeWeightSum = 0f;
		for (int i = 0; i < totalEnabledPills; i++) {
			cumulativeWeightSum += _enabledEffects[i].RarityMultiplier;
			if (randomRoll < cumulativeWeightSum) {
				return _enabledEffects[i];
			}
		}
		// I figure returning the last enabled element is a reasonable fallback...
		return _enabledEffects[totalEnabledPills - 1];
	}

	public static void ActivateEffect(Player player, IPillEffect selectedEffect) {
		float duration = selectedEffect.PossibleDurationRangeInclusive.Random;
		selectedEffect.OnEnabled(player, duration);
		string textToDisplay = AddDurationToHintText(selectedEffect.DisplayText, duration);
		if (textToDisplay.Length > 0) { // Some effects have no DisplayText because I needed better control.
			player.SendHint(textToDisplay);
		}
		if (duration < float.MaxValue - 1) {
			MEC.Timing.CallDelayed(duration, () => selectedEffect.OnDisabled(player)); // Can sometimes just do nothing if OnDisabled isn't overriden.
		}
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
		/// <summary>
		/// Used for a pill effect which permanently changes the player in certain ways.
		/// </summary>
		GoodAsPermanent = 1 << 3,
	}

	internal readonly struct Duration(float minimum, float maximum) {
		public float Random => UnityEngine.Random.Range(minimum, maximum);
	}
}

/// <summary>
/// This got kinda ugly so made a separate class for it. Its SOLE purpose is to populate a global static field
/// which contains all the pill effects.
/// </summary>
internal static class PillEffectPopulator {
	public static IPillEffect[] GetAllPillEffects() {
#if DEBUG
		return typeof(IPillEffect).Assembly.GetTypes()
			.Where(x => typeof(IDebugPickPills).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
			.Select(InitializeEffectRespectingConfigValues)
			.ToArray();
#else
		return typeof(IPillEffect).Assembly.GetTypes()
			.Where(x => typeof(IPillEffect).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
			.Select(InitializeEffectRespectingConfigValues)
			.ToArray();
#endif
	}

	private static IPillEffect InitializeEffectRespectingConfigValues(Type effectType) {
		PropertyInfo? configProperty = typeof(Config).GetProperty(effectType.Name, BindingFlags.Instance | BindingFlags.Public);

		object? matchingConfig = configProperty?.GetValue(Plugin.Singleton.Config);
		if (matchingConfig is not null) {
			ConstructorInfo? configCtor = effectType.GetConstructor([matchingConfig.GetType()]);
			if (configCtor is not null) {
				return (IPillEffect)configCtor.Invoke([matchingConfig]);
			}
		}
		throw new InvalidOperationException($"{effectType.Name} has incorrect/nonexistent config setup... this is a bug which needs to be fixed."); // TODO can this also happen if user manipulates the config file itselF?? I'd guess no.
	}
}
