namespace LuckyPills.Effects;

internal record TeslaGate : PillEffect {
	private readonly List<Tesla> _allTeslaGates = Map.Teslas?.ToList() ?? [];

	// Its essential to cause IsEnabled to be false if the map has no tesla gates for whatever reason.
	protected override bool IsEnabled => (_allTeslaGates.Count > 0 && true); // TODO the 2nd thing here should be loaded from config imo
	protected override string DisplayText { get; } = "You've been sent to a tesla gate!";

	protected override void OnEnabled(Player player, int duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		Tesla? tesla = _allTeslaGates.OrderBy(_ => UnityEngine.Random.value).FirstOrDefault();

		if (tesla is null) {
			Logger.Error("No tesla gates found in the map. This should have been prevented before this point.");
			return;
		}
		player.Position = tesla.Position;
	}
}