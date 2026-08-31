using MapGeneration;

namespace LuckyPills.Effects;

internal sealed class RoomWhichKillsYou(RoomWhichKillsYouConfig config) : IPillEffect {
	private static readonly Dictionary<Player, Room> _playersAndRoomsWhichKillThem = [];

	public bool IsEnabled(Player player) => !_playersAndRoomsWhichKillThem.ContainsKey(player) && config.IsEnabled;
	// No DisplayText because I want to display the zone as well, so I just display it in OnEnabled
	public float RarityMultiplier => config.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		FacilityZone zoneToUse = GetZoneToUse();
		Room roomToUse = GetRoomToUse(player.Room, zoneToUse);
		player.SendHint(GetHintText(roomToUse));
		_playersAndRoomsWhichKillThem.Add(player, roomToUse);
	}

	public void OnDisabled(Player player) {
		_playersAndRoomsWhichKillThem.Remove(player);
	}

	private static FacilityZone GetZoneToUse() => Random.value switch {
		<= 0.333f => FacilityZone.LightContainment,
		<= 0.7f => FacilityZone.HeavyContainment,
		_ => FacilityZone.Entrance
	};

	private static Room GetRoomToUse(Room? playerCurrentRoom, FacilityZone zoneToUse) {
		Room? roomToUse;
		do { // I don't want to roll the player's current room...
			roomToUse = GetRandomRoom(zoneToUse);
			if (roomToUse is null) {
				throw new InvalidOperationException("RoomWhichKillsYou effect activated with NO ROOMS IN THE MAP??? HCOW???");
			}
		} while (playerCurrentRoom == roomToUse);
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
		string hintText = $"A random room{zoneToDisplay}will kill you upon entry";
		return hintText;
	}

	// Looked like the LabApi version of this method was quite unoptimized. At least, the version in open source control was.
	private static Room? GetRandomRoom(FacilityZone zoneToUse) {
		Room[] rooms = Map.Rooms.Where(x => x.Zone == zoneToUse).ToArray();
		return rooms.Length == 0 ? null : rooms.RandomItem();
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
			MEC.Timing.CallDelayed(2.5f, player.BlowUp); // TODO I am testing 2.5 Maybe test 2 idk
		}
	}
}

internal sealed class RoomWhichKillsYouConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
