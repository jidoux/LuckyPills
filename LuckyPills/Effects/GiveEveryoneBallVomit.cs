namespace LuckyPills.Effects;

internal sealed class GiveEveryoneBallVomit : GiveEveryoneBallVomitConfig, IPillEffect {
	public new bool IsEnabled => new BallVomit().IsEnabled && base.IsEnabled;
	public string DisplayText => "You've given everyone ball vomit";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		BallVomit ballVomit = new();
		duration = 30f;
		foreach (Player anyPlayerInMap in Player.List.Where(x => x.IsAlive)) {
			SharedCode.EnablePillEffect(ballVomit, player, duration);
		}
	}
}

internal class GiveEveryoneBallVomitConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
