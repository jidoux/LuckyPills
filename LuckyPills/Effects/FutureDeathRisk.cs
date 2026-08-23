using System.Runtime.CompilerServices;

namespace LuckyPills.Effects;

internal sealed class FutureDeathRisk : FutureDeathRiskConfig, IPillEffect, IDebugPickPills {
	private static readonly Dictionary<Player, string> _playersToDie = [];
	private static readonly HashSet<string> _relevantEventHandlers = [];

	// The amount is arbitrary.
	public new bool IsEnabled(Player player) => _relevantEventHandlers.Count > 20 && !_playersToDie.ContainsKey(player) && base.IsEnabled;
	public string DisplayText { get; } = "One randomly-selected action you take will kill you";
	public new float RarityMultiplier => base.RarityMultiplier;
	public EffectCapabilities Capabilities { get; } = EffectCapabilities.CandidateForGiveAll;

	public void OnEnabled(Player player, float duration) {
		_playersToDie.Add(player, GetRandomEventHandler());
	}

	public void OnDisabled(Player player) {
		_playersToDie.Remove(player);
	}

	public void OnRoundEnd() {
		_playersToDie.Clear();
		// Intentionally not clearing the relevant event handlers... no reason they cant stay populated 
	}

	public static void FutureDeathRiskBehavior(Player? player, [CallerMemberName] string callerMethod = "") {
		LogCallIfDebug(callerMethod);
		if (player is null || _relevantEventHandlers.Add(callerMethod) || _playersToDie.Count == 0) {
			return;
		}
		try { // will be called by many random event handlers, so dont want it to throw
			if (!_playersToDie.TryGetValue(player, out string eventHandlerTiedToPlayer)) {
				return;
			}
			if (callerMethod == eventHandlerTiedToPlayer && _playersToDie.Remove(player)) {
				player.SendHint($"Your Painkillers decided you can't trigger \"{callerMethod}\", so now you die");
				MEC.Timing.CallDelayed(1.5f, player.Kill);
			}
		}
		catch (Exception ex) {
			Logger.Error(ex);
		}
	}

	private static string GetRandomEventHandler() => _relevantEventHandlers.ElementAt(Random.Range(0, _relevantEventHandlers.Count));
}

internal class FutureDeathRiskConfig {
	public bool IsEnabled { get; set; } = true;
	public float RarityMultiplier { get; set; } = 1f;
}
