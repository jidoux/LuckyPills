using Interactables.Interobjects.DoorUtils;

namespace LuckyPills.Effects;

internal sealed class DestroyAllDoors(DestroyAllDoorsConfig config) : IPillEffect {
	private static bool _wasEveryDoorBrokenAlready = false;

	public bool IsEnabled(Player player) => !_wasEveryDoorBrokenAlready && config.IsEnabled;
	public string DisplayText { get; } = "You've destroyed every door";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, int duration) {
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
			if (doorVariant is Interactables.Interobjects.PryableDoor pryableDoor) {
				// TODO add this for the surface gates open and destroy them also 914 and 2 peanut spawns
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

internal sealed class DestroyAllDoorsConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 55;
}
