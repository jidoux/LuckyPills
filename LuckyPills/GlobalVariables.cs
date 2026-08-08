namespace LuckyPills;

internal static class GlobalVariables {
	public static readonly HashSet<Player> PlayersWhoCanOnlyPickUpPillsForTheRestOfTheGame = [];
	public static bool WasEveryDoorBrokenAlready { get; set; } = false;
}
