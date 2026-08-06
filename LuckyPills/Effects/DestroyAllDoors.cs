using Interactables.Interobjects.DoorUtils;

namespace LuckyPills.Effects;

internal sealed class DestroyAllDoors : DestroyAllDoorsConfig, IPillEffect {
	public new bool IsEnabled(Player player) => !GlobalVariables.WasEveryDoorBrokenAlready && base.IsEnabled;
	public string DisplayText => "You've destroyed every door";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		GlobalVariables.WasEveryDoorBrokenAlready = true;
		foreach (DoorVariant doorVariant in Map.Doors.Select(x => x.Base)) {
			if (doorVariant is Interactables.Interobjects.BreakableDoor damagableDoor) {
				damagableDoor.ServerDamage(float.MaxValue, DoorDamageType.ServerCommand);
			}
			else if (doorVariant is Interactables.Interobjects.CheckpointDoor checkpointDoor) {
				checkpointDoor.ServerDamage(float.MaxValue, DoorDamageType.ServerCommand);
			}
		}
	}
}

internal class DestroyAllDoorsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
