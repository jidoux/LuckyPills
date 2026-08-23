global using LabApi.Features.Wrappers;
global using Logger = LabApi.Features.Console.Logger;
global using UnityEngine;
global using Random = UnityEngine.Random;
global using static LuckyPills.PillEffectOrchestrator;
global using static LuckyPills.SharedCode;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;

namespace LuckyPills;

internal sealed class Plugin : Plugin<Config> {
	public static Plugin Singleton { get; private set; } = null!; // Used in case I need to access the config internally/globally.

	public override string Name => "LuckyPills";

	public override string Description => "Using Painkillers has some strange effects";

	public override string Author => "burnout__";

	// The current version of the plugin
	public override Version Version => new(1, 0, 0, 0);

	// The required version of LabAPI (usually the version the plugin was built with)
	public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);

	public override LoadPriority Priority => LoadPriority.Medium; // Its LoadPriority.Medium by default; I figure explicitness is fine.

	public EventHandlers Events { get; } = new();


	public override void Enable() {
		Singleton = this;
		CustomHandlersManager.RegisterEventsHandler(Events);
	}

	public override void Disable() {
		CustomHandlersManager.UnregisterEventsHandler(Events);
		Singleton = null!;
	}
}
