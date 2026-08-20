using MapGeneration;

namespace LuckyPills.Effects;

internal sealed class RoomWhichKillsYou : RoomWhichKillsYouConfig, IPillEffect, IDebugPickPills {
	private static readonly Dictionary<Player, Room> _playersAndRoomsWhichKillThem = [];

	public new bool IsEnabled(Player player) => !_playersAndRoomsWhichKillThem.ContainsKey(player) && base.IsEnabled;
	// No DisplayText because I want to display the zone as well, so I just display it in OnEnabled
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		FacilityZone zoneToUse = GetZoneToUse();
		Room roomToUse = GetRoomToUse(player.Room, zoneToUse);
		player.SendHint(GetHintText(roomToUse));
		_playersAndRoomsWhichKillThem.Add(player, roomToUse);
	}

	private static FacilityZone GetZoneToUse() => Random.value switch {
		<= 0.333f => FacilityZone.LightContainment,
		<= 0.7f => FacilityZone.HeavyContainment,
		_ => FacilityZone.Entrance
	};

	private static Room GetRoomToUse(Room? playerCurrentRoom, FacilityZone zoneToUse) {
		Room? roomToUse;
		do { // I don't want to roll the player's current room...
			roomToUse = Map.GetRandomRoom(zoneToUse);
			if (roomToUse is null) {
				throw new InvalidOperationException("RoomWhichKillsYou effect activated with NO ROOMS IN THE MAP??? HCOW???");
			}
		} while (playerCurrentRoom != roomToUse && playerCurrentRoom is not null); // But if the player's current room is null, we can just exit the loop.
		return roomToUse;
	}

	private static string GetHintText(Room roomToUse) {
		// NOTE this is weird with spaces and such, so be careful.
		string zoneToDisplay = roomToUse.Zone switch {
			FacilityZone.LightContainment => " in the Light Containment Zone ",
			FacilityZone.HeavyContainment => " in the Heavy Containment Zone ",
			FacilityZone.Entrance => " in the Entrance Zone ",
			FacilityZone.Surface => " on Surface ",
			_ => " somewhere "
		};
		string hintText = $"A random room{zoneToDisplay}will kill you upon entry");
		return hintText;
	}

	public void OnRoundEnd() {
		_playersAndRoomsWhichKillThem.Clear();
	}

	public static void PlayerEnteredRoom(Player player, Room? newRoom) {
		// Its important that the player gets removed from the dictionary if they are killed.
		if (_playersAndRoomsWhichKillThem.TryGetValue(player, out Room roomAssociatedWithPlayer)
			&& newRoom is not null
			&& newRoom == roomAssociatedWithPlayer
			&& _playersAndRoomsWhichKillThem.Remove(player)) {
			player.SendHint($"Your Painkillers decided you can't enter this room, so now you die");
			MEC.Timing.CallDelayed(1.5f, player.Kill);
		}
	}
}

internal class RoomWhichKillsYouConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
