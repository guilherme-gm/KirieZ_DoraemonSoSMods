using BepInEx.Logging;

/**
 * Wrapper to call BepInEx logger.
 *
 * PluginLogger.Logger will be set at start up so everything else may use it.
 */
internal static class PluginLogger {
	internal static ManualLogSource Logger;

	internal static void LogDebug(object data)
	{
		Logger?.LogDebug(data);
	}

	internal static void LogError(object data)
	{
		Logger?.LogError(data);
	}

	internal static void LogFata(object data)
	{
		Logger?.LogFatal(data);
	}

	internal static void LogInfo(object data)
	{
		Logger?.LogInfo(data);
	}

	internal static void LogMessage(object data)
	{
		Logger?.LogMessage(data);
	}

	internal static void LogWarning(object data)
	{
		Logger?.LogWarning(data);
	}
}
