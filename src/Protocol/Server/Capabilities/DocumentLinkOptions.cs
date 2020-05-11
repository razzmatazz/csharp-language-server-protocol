using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Server.Capabilities
{
    /// <summary>
    ///  Document link options
    /// </summary>
    public class DocumentLinkOptions : WorkDoneProgressOptions, IDocumentLinkOptions
    {
        /// <summary>
        ///  Document links have a resolve provider as well.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public bool ResolveProvider { get; set; }

        public static DocumentLinkOptions Of(IDocumentLinkOptions options, IEnumerable<IHandlerDescriptor> descriptors)
        {
            return new DocumentLinkOptions() {
                ResolveProvider = options.ResolveProvider  || descriptors.Any(z => z.HandlerType == typeof(IDocumentLinkResolveHandler)) ,
                WorkDoneProgress = options.WorkDoneProgress,
            };
        }
    }
}
