namespace LuckyPills.Effects;

internal sealed record TeslaGate : TeslaGateConfig, IPillEffect {
	public new bool IsEnabled => Map.Teslas.Any() && base.IsEnabled;
	public string DisplayText => "You've been sent to a tesla gate";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Tesla? tesla = Map.Teslas.OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();

		if (tesla is null) {
			// May be possible with some crazy timing where this gets executed right when the map ends, but idk.
			Logger.Error("No tesla gates found in the map. Odds are this should have been prevented before this point.");
			return;
		}
		player.Position = tesla.Position + Vector3.up;
	}
}

internal record TeslaGateConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
