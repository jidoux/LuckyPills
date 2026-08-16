using Interactables.Interobjects.DoorUtils;

namespace LuckyPills.Effects;

internal sealed class DestroyAllDoors : DestroyAllDoorsConfig, IPillEffect {
	private static bool _wasEveryDoorBrokenAlready = false;

	public new bool IsEnabled(Player player) => !_wasEveryDoorBrokenAlready && base.IsEnabled;
	public string DisplayText => "You've destroyed every door";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		DestroyAllTheDoors();
	}

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

	public void OnRoundEnded() {
		_wasEveryDoorBrokenAlready = false;
	}
}

internal class DestroyAllDoorsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.6f;
}
