﻿using System.Text.Json.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities
{
    /// <summary>
    /// Specific capabilities for the `SymbolKind`.
    /// </summary>
    public class SymbolKindCapability
    {
        /// <summary>
        /// The symbol kind values the client supports. When this
        /// property exists the client also guarantees that it will
        /// handle values outside its set gracefully and falls back
        /// to a default value when unknown.
        ///
        /// If this property is not present the client only supports
        /// the symbol kinds from `File` to `Array` as defined in
        /// the initial version of the protocol.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public Container<SymbolKind> ValueSet { get; set; }
    }
}
