using InventorySystem;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;

namespace LuckyPills.Effects;

internal sealed class ScrambleRolesAndItems : ScrambleRolesAndItemsConfig, IPillEffect, IDebugPickPills {
	public new bool IsEnabled(Player player) => Player.List.Count(x => x.IsAlive) > 1 && base.IsEnabled;
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		PreviousPlayer prevPlayer = new(
			player.Items.Select(x => x.Base.ItemTypeId).ToList(),
			new Dictionary<ItemType, ushort>(player.Ammo),
			player.Role,
			player.Health,
			player.Position,
			player.Rotation,
			player.LookRotation,
			Scp079ExpTotal: null
		);
		PreviousPlayer nextPrevPlayer;
		Player currPlayer;
		Player[] playerArray = Player.List.Where(x => x.IsAlive && x != player).ToArray();
		for (int i = 0; i < playerArray.Length; i++) {
			// The strategy is to cache the prevPlayer/firstPlayer and do operations on currPlayer.
			// then at the very end, do operations on player itself on the very last player.
			currPlayer = playerArray[i];

			nextPrevPlayer = new(
				currPlayer.Items.Select(x => x.Base.ItemTypeId).ToList(),
				new Dictionary<ItemType, ushort>(currPlayer.Ammo),
				currPlayer.Role,
				currPlayer.Health,
				currPlayer.Position,
				currPlayer.Rotation,
				currPlayer.LookRotation,
				SharedCode.TryGetScp079TierManager(currPlayer.RoleBase, out Scp079TierManager? tierManager) ? tierManager?.TotalExp : null
			);
			SetPlayerState(currPlayer, prevPlayer);
			prevPlayer = nextPrevPlayer;
		}
		// Now doing the very last case which is the current player.
		SetPlayerState(player, prevPlayer);
	}

	private static void SetPlayerState(Player currPlayer, PreviousPlayer prevPlayer) {
		currPlayer.SetRole(prevPlayer.Role, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
		currPlayer.ClearInventory(clearAmmo: true, clearItems: true);
		currPlayer.Health = prevPlayer.Health;
		currPlayer.Position = prevPlayer.Position;
		currPlayer.Rotation = prevPlayer.Rotation;
		currPlayer.LookRotation = prevPlayer.LookRotation;
		if (prevPlayer.Scp079ExpTotal is not null && SharedCode.TryGetScp079TierManager(currPlayer.RoleBase, out Scp079TierManager? currTierManager)) {
			SharedCode.SetScp079ExpLevel(currTierManager, prevPlayer.Scp079ExpTotal.Value);
		}
		foreach (KeyValuePair<ItemType, ushort> ammo in prevPlayer.AllAmmo) {
			currPlayer.Inventory.ServerAddAmmo(ammo.Key, ammo.Value);
		}
		// I think need to add the items after ammo so that it can handle oversized item additions.
		foreach (ItemType item in prevPlayer.AllItems) {
			currPlayer.Inventory.ServerAddItem(item, InventorySystem.Items.ItemAddReason.AdminCommand);
		}
		currPlayer.SendHint("You wake up from a dream... maybe this is who you've always been...?");
	}

	/// <summary>
	/// I'd think this will work as a "player deep copy" mechanism since the overriden properties of the player
	/// that populated this object are simply overridden.
	/// </summary>
	/// <remarks>
	/// Could add HumeShield, ArtificialHealth, StaminaRemaining, ActiveEffects. The ArtificialHealth is
	/// given by Adrenaline for example, and I noticed just copying the value didn't copy over the natural drain
	/// that should accompany it. I removed HumeShield preemptively for fear of same situation. I remove ActiveEffects
	/// just because I couldn't get it to work for some reason... not really sure why.
	/// </remarks>
	private sealed record PreviousPlayer(
		IReadOnlyCollection<ItemType> AllItems,
		IReadOnlyDictionary<ItemType, ushort> AllAmmo,
		RoleTypeId Role,
		float Health,
		Vector3 Position,
		Quaternion Rotation,
		Vector2 LookRotation,
		int? Scp079ExpTotal
	);
}

internal class ScrambleRolesAndItemsConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
