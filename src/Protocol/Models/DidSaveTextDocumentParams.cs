using System.Text.Json.Serialization;
using MediatR;
using OmniSharp.Extensions.JsonRpc;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;

namespace OmniSharp.Extensions.LanguageServer.Protocol.Models
{
    [Method(DocumentNames.DidSave)]
    public class DidSaveTextDocumentParams : ITextDocumentIdentifierParams, IRequest
    {
        /// <summary>
        ///  The document that was saved.
        /// </summary>
        public TextDocumentIdentifier TextDocument { get; set; }

        /// <summary>
        ///  Optional the content when saved. Depends on the includeText value
        ///  when the save notifcation was requested.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenNull)]
        public string Text { get; set; }
    }
}
