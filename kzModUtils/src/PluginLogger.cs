using System.Diagnostics;
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
		var stackTrace = new StackTrace(1);

		Logger?.LogError(data);
		Logger?.LogError(stackTrace.ToString());
	}

	internal static void LogFatal(object data)
	{
		var stackTrace = new StackTrace(1);

		Logger?.LogFatal(data);
		Logger?.LogFatal(stackTrace.ToString());
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
