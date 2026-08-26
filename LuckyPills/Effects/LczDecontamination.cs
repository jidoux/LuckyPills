using LightContainmentZoneDecontamination;
using MapGeneration;

namespace LuckyPills.Effects;

internal sealed class LczDecontamination(LczDecontaminationConfig config) : IPillEffect {
	public bool IsEnabled(Player player) =>
		player.Zone == FacilityZone.LightContainment && DecontaminationController.Singleton.TimeOffset < 600f && config.IsEnabled;
	public string DisplayText { get; } = "You've triggered Light Containment Zone decontamination";
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		// must be below 666 and above 660... I didn't test 665. Just an arbitrary offset to trigger 30 second countdown.
		float timeUntilDecontam = 663f;
		float offsetFromStartTime = timeUntilDecontam - (float)Round.Duration.TotalSeconds;
		DecontaminationController.Singleton.TimeOffset += offsetFromStartTime;
	}
}

internal sealed class LczDecontaminationConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.4f;
}
