using System.Text.Json.Serialization;
using OmniSharp.Extensions.DebugAdapter.Protocol.Models;
using OmniSharp.Extensions.DebugAdapter.Protocol.Serialization;
using MediatR;

namespace OmniSharp.Extensions.DebugAdapter.Protocol.Requests
{
    public class SetExceptionBreakpointsArguments : IRequest<SetExceptionBreakpointsResponse>
    {
        /// <summary>
        /// IDs of checked exception options.The set of IDs is returned via the 'exceptionBreakpointFilters' capability.
        /// </summary>
        public Container<string> Filters { get; set; }

        /// <summary>
        /// Configuration options for selected exceptions.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)] public Container<ExceptionOptions> ExceptionOptions { get; set; }
    }

}