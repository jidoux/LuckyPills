namespace LuckyPills.Effects;

internal sealed class NextRoundNoPills : NextRoundNoPillsConfig, IPillEffect {
	private static bool _notSpawningPillsNextRound = false;
	private static bool _notSpawningPillsThisRound = false;

	public new bool IsEnabled(Player player) => !GlobalVariables.IsSpecialEventHappeningNextRound &&
		!_notSpawningPillsNextRound && base.IsEnabled;
	public string DisplayText => "Something special will happen next round...";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		GlobalVariables.IsSpecialEventHappeningNextRound = true;
		_notSpawningPillsNextRound = true;
	}

	public static bool ShouldNotSpawnPills(ItemType itemType) {
		return _notSpawningPillsThisRound && (itemType == ItemType.Painkillers || itemType == ItemType.Medkit || itemType == ItemType.Adrenaline);
	}

	public void OnRoundEnd() {
		if (_notSpawningPillsThisRound) {
			_notSpawningPillsThisRound = false;
			_notSpawningPillsNextRound = false;
		}
		else if (_notSpawningPillsNextRound) {
			_notSpawningPillsThisRound = true;
			GlobalVariables.IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal class NextRoundNoPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 0.5f;
}
