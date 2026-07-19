//namespace LuckyPills.Effects;

// //TODO - implement this when you figure out how to . Also do a tantrum one if you can figure that out.
//internal record TheFog : PillEffect, IDebugPickPills {
//	protected override bool IsEnabled { get; } = true;
//	protected override string DisplayText { get; } = "You've created the fog";

//	protected override void OnEnabled(Player player, float duration) {
//		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
//		ItemType itemType = ItemType.SCP244a;
//		if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5) {
//			itemType = ItemType.SCP244b;
//		}
//		player.Gravity = new UnityEngine.Vector3

//		ExplosiveGrenadeProjectile.SpawnActive(player.Position, itemType, timeOverride: 0.001);

//	}
//}
