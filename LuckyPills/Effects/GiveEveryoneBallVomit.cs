namespace LuckyPills.Effects;

internal sealed class GiveEveryoneBallVomit : GiveEveryoneBallVomitConfig, IPillEffect {
	private static readonly BallVomit _ballVomitInstance = new();

	public new bool IsEnabled(Player player) => _ballVomitInstance.IsEnabled(player) && base.IsEnabled;
	public string DisplayText => "You've given everyone ball vomit";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		duration = 30f;
		foreach (Player anyPlayerInMap in Player.List.Where(x => x.IsAlive)) {
			SharedCode.EnablePillEffect(_ballVomitInstance, player, duration);
		}
	}
}

internal class GiveEveryoneBallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
