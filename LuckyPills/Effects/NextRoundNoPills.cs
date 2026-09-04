namespace LuckyPills.Effects;

internal sealed class NextRoundNoPills(NextRoundNoPillsConfig config) : IPillEffect {
	private static bool _notSpawningPillsNextRound = false;
	private static bool _notSpawningPillsThisRound = false;

	public bool IsEnabled(Player player) => !IsSpecialEventHappeningNextRound &&
		!_notSpawningPillsNextRound && config.IsEnabled;
	public string DisplayText { get; } = "Something special will happen next round...";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		IsSpecialEventHappeningNextRound = true;
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
			IsSpecialEventHappeningNextRound = false;
		}
	}
}

internal sealed class NextRoundNoPillsConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 25;
}
