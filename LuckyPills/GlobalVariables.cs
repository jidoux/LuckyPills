namespace LuckyPills;

internal static class GlobalVariables {
	public static readonly List<Player> PlayersWhoCanOnlyPickUpPillsForTheRestOfTheGame = [];
	public static bool WasEveryDoorBrokenAlready { get; set; } = false;
}
