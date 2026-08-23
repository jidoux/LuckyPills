namespace LuckyPills.Effects;

internal sealed class PermanentPlayerChanges : PermanentPlayerChangesConfig, IPillEffect {
	private static readonly HashSet<Player> _playersWithPermanentEffectsThisRound = [];
	private readonly Lazy<IPillEffect[]> _effectCandidates = new(() => {
		// I did this mainly because I was scared of static initialization order issues with my singleton AllPillEffects,
		// but this also may be better to not allocate this static field, to be honest.
		return AllPillEffects
			.Where(x => (x.Capabilities & EffectCapabilities.GoodAsPermanent) == EffectCapabilities.GoodAsPermanent)
			.ToArray();
	});

	// I'm not letting players get 2 permanent effects at once mostly because I don't feel like
	// tracking for duplicates.
	public new bool IsEnabled(Player player) => !_playersWithPermanentEffectsThisRound.Contains(player) && base.IsEnabled;
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		_playersWithPermanentEffectsThisRound.Add(player);
		// Prob should respect the config's enabled settings. The tradeoff being that if enough are disabled
		// in the config, nothing will happen. Oh well...
		IPillEffect[] enabledCandidates = _effectCandidates.Value.Where(x => x.IsEnabled(player)).ToArray();
		if (enabledCandidates.Length == 0) {
			Logger.Warn("There are no pills enabled which qualify for PermanentPlayerChanges. Aborting pill...");
			return;
		}
		// IPillEffect randomlySelectedEffect = candidates.ElementAt(Random.Range(0, candidates.Count));
		IPillEffect randomlySelectedEffect = enabledCandidates[Random.Range(0, enabledCandidates.Length)];
		randomlySelectedEffect.OnEnabled(player, 3600f);
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
