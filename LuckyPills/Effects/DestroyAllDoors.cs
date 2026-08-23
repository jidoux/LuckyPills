using Interactables.Interobjects.DoorUtils;

namespace LuckyPills.Effects;

internal sealed class DestroyAllDoors : DestroyAllDoorsConfig, IPillEffect {
	private static bool _wasEveryDoorBrokenAlready = false;

	public new bool IsEnabled(Player player) => !_wasEveryDoorBrokenAlready && base.IsEnabled;
	public string DisplayText { get; } = "You've destroyed every door";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		DestroyAllTheDoors();
	}

	// Doing weird stuff here to avoid manipulating the static bool from a non-static method. Unsure yet of how
	// relevant that actually is since idk how many threads are actually active for LabAPI plugins.
	private static void DestroyAllTheDoors() {
		_wasEveryDoorBrokenAlready = true;
		foreach (DoorVariant doorVariant in Map.Doors.Select(x => x.Base)) {
			if (doorVariant is Interactables.Interobjects.BreakableDoor damagableDoor) {
				damagableDoor.ServerDamage(float.MaxValue, DoorDamageType.ServerCommand);
			}
			else if (doorVariant is Interactables.Interobjects.CheckpointDoor checkpointDoor) {
				checkpointDoor.ServerDamage(float.MaxValue, DoorDamageType.ServerCommand);
			}
		}
	}

	private static void UnsetDestroyedDoors() {
		_wasEveryDoorBrokenAlready = false;
	}

	public void OnRoundEnd() {
		UnsetDestroyedDoors();
	}
}

internal class DestroyAllDoorsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.6f;
}
