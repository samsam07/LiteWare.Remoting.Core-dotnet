using LiteWare.Remoting.Core.Fluent.RemoteServiceDefinitions;

namespace LiteWare.Remoting.Core;

/// <summary>
/// Allows the configuration and building of remote services using fluent language.
/// </summary>
public static class Remote
{
    /// <summary>
    /// Configures a new remote service using a fluent language definition.
    /// </summary>
    /// <returns>The entry point for the fluent remote service definition.</returns>
    public static IRemoteServiceDefinition ConfigureService() =>
        new RemoteServiceDefinition();
}