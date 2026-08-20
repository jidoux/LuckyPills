namespace LuckyPills.Effects;

internal sealed class PermanentPlayerChanges : PermanentPlayerChangesConfig, IPillEffect, IDebugPickPills {
	private static readonly HashSet<Player> _playersWithPermanentEffectsThisRound = [];
	private static readonly IReadOnlyCollection<IPillEffect> _effectCandidates = GetAllPillEffects()
		.Where(x => (x.Capabilities & EffectCapabilities.GoodAsPermanent) == EffectCapabilities.GoodAsPermanent)
		.ToList().AsReadOnly(); // TODO check out this linq as im not sure whats happening perf wise... is better to lazy evaluate orrrr?? I do need another enumerable so idek.

	public new bool IsEnabled(Player player) => !_playersWithPermanentEffectsThisRound.Contains(player) && base.IsEnabled;
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		_playersWithPermanentEffectsThisRound.Add(player);
		List<IPillEffect> candidates = _effectCandidates.Where(x => x.IsEnabled(player)).ToList(); // Prob should respect the config's enabled settings.
		//IPillEffect randomlySelectedEffect = candidates.ElementAt(Random.Range(0, candidates.Count));
		IPillEffect randomlySelectedEffect = candidates[Random.Range(0, candidates.Count)];
		randomlySelectedEffect.OnEnabled(player, 0f);
		// yeahhh this is kinda bad/scary approach, but its simplest.
		string hintToSend = randomlySelectedEffect.DisplayText.Replace("for {duration} seconds", "permanently", StringComparison.Ordinal);
		player.SendHint(hintToSend);
	}

	public void OnRoundEnd() {
		_playersWithPermanentEffectsThisRound.Clear();
	}
}

internal class PermanentPlayerChangesConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
