namespace LuckyPills.Effects;

internal sealed record TheFog : TheFogConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled => base.IsEnabled;
	public string DisplayText => "You've created the fog";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
		//ItemType itemType = ItemType.SCP244a;
		//if (Random.Range(0.0f, 1.0f) > 0.5) {
		//	itemType = ItemType.SCP244b;
		//}

		var item = Scp244Item.Get(new InventorySystem.Items.Usables.Scp244.Scp244Item());
		if (item is not null) {
			item.DropItem();
			item.Use();
		}
		//Pickup? pickup = Pickup.Create(type: itemType, position: player.Position, rotation: Quaternion.identity, scale: Vector3.one);
		//if (pickup != null) {
		//pickup.IsInUse = true;
		//}
		// TODO - make this work ok?
		//pickup?.GameObject.gameObject.SetActive(true);
	}
}

internal record TheFogConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
