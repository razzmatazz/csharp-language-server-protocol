using System.Text.Json.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities
{
    /// <summary>
    /// Capabilities specific to `textDocument/publishDiagnostics`.
    /// </summary>
    public class PublishDiagnosticsCapability
    {
        /// <summary>
        /// Whether the clients accepts diagnostics with related information.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public bool RelatedInformation { get; set; }

        /// <summary>
        /// Client supports the tag property to provide meta data about a diagnostic.
        /// Clients supporting tags have to handle unknown tags gracefully.
        ///
        /// @since 3.15.0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public Supports<PublishDiagnosticsTagSupportCapability> TagSupport { get; set; }

        /// <summary>
        /// Whether the client interprets the version property of the
        /// `textDocument/publishDiagnostics` notification's parameter.
        ///
        /// @since 3.15.0
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public bool VersionSupport { get; set; }
    }
}