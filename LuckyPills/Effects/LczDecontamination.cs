using LightContainmentZoneDecontamination;
using MapGeneration;

namespace LuckyPills.Effects;

internal sealed class LczDecontamination : LczDecontaminationConfig, IPillEffect {
	public new bool IsEnabled(Player player) =>
		player.Zone == FacilityZone.LightContainment && DecontaminationController.Singleton.TimeOffset < 600f && base.IsEnabled;
	public string DisplayText { get; } = "You've triggered Light Containment Zone decontamination";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		// must be below 666 and above 660... I didn't test 665. Just an arbitrary offset to trigger 30 second countdown.
		float timeUntilDecontam = 663f;
		float offsetFromStartTime = timeUntilDecontam - (float)Round.Duration.TotalSeconds;
		DecontaminationController.Singleton.TimeOffset += offsetFromStartTime;
	}
}

internal class LczDecontaminationConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.4f;
}
