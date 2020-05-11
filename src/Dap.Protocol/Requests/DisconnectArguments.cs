using System.Text.Json.Serialization;
using OmniSharp.Extensions.DebugAdapter.Protocol.Serialization;
using MediatR;

namespace OmniSharp.Extensions.DebugAdapter.Protocol.Requests
{
    public class DisconnectArguments : IRequest<DisconnectResponse>
    {
        /// <summary>
        /// A value of true indicates that this 'disconnect' request is part of a restart sequence.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)] public bool? Restart { get; set; }

        /// <summary>
        /// Indicates whether the debuggee should be terminated when the debugger is disconnected.
        /// If unspecified, the debug adapter is free to do whatever it thinks is best.
        /// A client can only rely on this attribute being properly honored if a debug adapter returns true for the 'supportTerminateDebuggee' capability.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)] public bool? TerminateDebuggee { get; set; }
    }

}
