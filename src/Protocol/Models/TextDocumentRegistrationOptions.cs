using System.Text.Json.Serialization;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Models
{
    public class TextDocumentRegistrationOptions : ITextDocumentRegistrationOptions
    {
        /// <summary>
        ///  A document selector to identify the scope of the registration. If set to null
        ///  the document selector provided on the client side will be used.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public DocumentSelector DocumentSelector { get; set; }
    }
}
