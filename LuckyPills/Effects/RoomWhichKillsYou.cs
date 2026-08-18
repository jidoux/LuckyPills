using MapGeneration;

namespace LuckyPills.Effects;

internal sealed class RoomWhichKillsYou : RoomWhichKillsYouConfig, IPillEffect, IDebugPickPills {
	private static readonly Dictionary<Player, Room> _playersAndRoomsWhichKillThem = [];

	public new bool IsEnabled(Player player) => !_playersAndRoomsWhichKillThem.ContainsKey(player) && base.IsEnabled;
	// No DisplayText because I want to display the zone as well, so I just display it in OnEnabled
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities => EffectCapabilities.None;

	public void OnEnabled(Player player, float duration) {
		Room? roomToUse;
		do { // I don't want to roll the player's current room...
			roomToUse = Map.GetRandomRoom();
			if (roomToUse is null) {
				Logger.Warn("RoomWhichKillsYou effect activated with NO ROOMS IN THE MAP??? HCOW???");
				return;
			}
		} while (player.Room != roomToUse);
		// NOTE this is weird with spaces and such, so be careful.
		string zoneToDisplay = roomToUse.Zone switch {
			FacilityZone.LightContainment => " in the Light Containment Zone ",
			FacilityZone.HeavyContainment => " in the Heavy Containment Zone ",
			FacilityZone.Entrance => " in the Entrance Zone ",
			FacilityZone.Surface => " on Surface ",
			_ => " somewhere "
		};
		player.SendHint($"A random room{zoneToDisplay}will kill you upon entry");
		_playersAndRoomsWhichKillThem.Add(player, roomToUse);
	}

	public void OnRoundEnd() {
		_playersAndRoomsWhichKillThem.Clear();
	}

	public static void PlayerEnteredRoom(Player player, Room? newRoom) {
		if (_playersAndRoomsWhichKillThem.TryGetValue(player, out Room roomAssociatedWithPlayer)
			&& newRoom is not null
			&& newRoom == roomAssociatedWithPlayer) {
			player.SendHint($"Your Painkillers decided you can't enter this room, so now you die");
			MEC.Timing.CallDelayed(1.5f, player.Kill);
		}
	}
}

internal class RoomWhichKillsYouConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
