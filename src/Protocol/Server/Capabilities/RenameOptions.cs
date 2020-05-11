using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities
{
    public class RenameOptions : WorkDoneProgressOptions, IRenameOptions
    {
        /// <summary>
        /// Renames should be checked and tested before being executed.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public bool PrepareProvider { get; set; }

        public static RenameOptions Of(IRenameOptions options, IEnumerable<IHandlerDescriptor> descriptors)
        {
            return new RenameOptions() {
                PrepareProvider = options.PrepareProvider || descriptors.Any(z => z.HandlerType == typeof(IPrepareRenameHandler)),
                WorkDoneProgress = options.WorkDoneProgress
            };
        }
    }
}
