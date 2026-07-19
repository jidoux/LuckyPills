//namespace LuckyPills.Effects;

// TODO - this one puts numbers on your screen count down to die
// Also 2 one does "some actoin will cause you to blow up" and doesnt tell you which action it man
// Also 3 one where it randomly changes your gravity over the next 30 seconds.
// ALso 4 one where it gives you a list of people and you choose who to smite
// Also 5 one which makes it so that anything you pick up for the next of the game automatically turns into pills
// Also 6 one where it spawns everyone to peanut spawn and turns everyone into peanut and its an extremely rare chance
// Also 7 one which closes teh game and puts everyone into a valheim server genuinely one which closes the game
// ALso 8 remove hte cap on 3 medical items. Mciahel says remove any cap on anything. So maybe make a mod which does that also.
// Also 9 one which does all of the effects
// Also 9.5 one which does every vomit
// Also 10 one which doesnt do anything but spawns all the class D with AKs next round
// ALso 11 one which starts the nuke process
// Also 12 Make one of the pills put 50 bucks in nikos bank account
// also 13 pill that blows up every door
// also 14 rare one that gives everyone ball vomit
// also 15 one where if you are looking at a guy, or next to a guy, it blows them up
// also 16 add pink candy back
// also 17 electrocute you zip with the efrect of a tesla game
// also 18 make you become a mobile tesla gate
// alsao 19 make one that makes you the puter for hte rest of the game and makes you max level
//internal record DeathCounter : PillEffect {
//	private const int _grenadesPerSecond = 10; // I'd prefer having this here rather than grenades class for potential fine tuning.

//	protected override bool IsEnabled { get; } = true;
//	protected override string DisplayText { get; } = "You've been given bomb vomit for {duration} seconds";
//	protected override Duration PossibleDurationRangeInclusive { get; } = new(10f, 20f);

//	protected override void OnEnabled(Player player, float duration) {
//		Logger.Debug($"{this.GetType().Name} {System.Reflection.MethodBase.GetCurrentMethod().Name}");
//		MEC.Timing.RunCoroutine(Grenades.RunGrenadeVomit(player, duration, _grenadesPerSecond, ItemType.GrenadeHE));
//	}
//}
