using PlayerRoles;

namespace LuckyPills.Effects;

internal sealed class GiveEveryoneAks(GiveEveryoneAksConfig config) : IPillEffect, IDebugPickPills {
	public bool IsEnabled(Player player) => config.IsEnabled;
	public string DisplayText { get; } = "You've given every player an AK";
	public ushort RarityWeight => config.RarityWeight;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, int duration) {
		foreach (Player anyPlayerInMap in Player.ReadyList.Where(x => x.IsAlive && x.Team != Team.SCPs)) {
			if (anyPlayerInMap.Items.Count() >= 8) {
				//SpawnFirearmBelow(player.Position, ItemType.GunAK); // TODO need to do this still
			}
			else {
				if (anyPlayerInMap != player) {
					anyPlayerInMap.SendHint("You've been given an AK by someone else's Painkillers", duration: 4);
				}
				anyPlayerInMap.ForceEquip(ItemType.GunAK);
			}
		}
	}

	// TODO - I have no idea what im doing LOL. In theory I'd think the ServerModifyAmmo should work but clearly that din't happen soooo idk

	//private static void SpawnFirearmBelow(Vector3 position, ItemType itemType) {
	//	FirearmPickup? firearm = (FirearmPickup?)FirearmPickup.Create(itemType, position);
	//	Logger.Debug("is firearm null? " + firearm is null);
	//	//if (firearm is not null && firearm.Base.Template.TryGetModule<IPrimaryAmmoContainerModule>(out IPrimaryAmmoContainerModule? ammoModule)) {
	//	//	Logger.Debug("We got into the cond so we have ammo module" + ammoModule.AmmoType + ammoModule.AmmoMax);
	//	//	ammoModule.ServerModifyAmmo(5);
	//	//}
	//	if (firearm is not null) {
	//		Logger.Debug("Firearm is not null ok? lets co ntinue");
	//		InventorySystem.Items.Firearms.FirearmWorldmodel worldModel = firearm.Base.Template.WorldModel;
	//		var extensions = worldModel.Extensions;
	//		Logger.Debug("Extensions len: " + extensions.Length);
	//		foreach (var extension in extensions) {
	//			Logger.Debug("extension info: " + extension.name);
	//		}
	//		Logger.Debug("Firearm is not null ok? lets co ntinue");
	//		InventorySystem.Items.Firearms.FirearmWorldmodel worldModel1 = firearm.Base.Worldmodel;
	//		var extensions1 = worldModel.Extensions;
	//		Logger.Debug("Extensions len: " + extensions1.Length);
	//		foreach (var extension1 in extensions1) {
	//			Logger.Debug("extension info: " + extension1.name);
	//		}
	//	}
	//}
}

internal sealed class GiveEveryoneAksConfig {
	public bool IsEnabled { get; set; } = true;
	public ushort RarityWeight { get; set; } = 80;
}
