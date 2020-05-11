using System.Text.Json;
using FluentAssertions;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using Xunit;
#pragma warning disable 618

namespace Lsp.Tests
{
    public class WorkspaceSymbolKindTests
    {
        [Fact]
        public void DefaultBehavior_Should_Only_Support_InitialSymbolKinds()
        {

            var json = JsonSerializer.Serialize(new SymbolInformation()
            {
                Kind = SymbolKind.Event
            }, Serializer.Instance.Options);

            var result = JsonSerializer.Deserialize<SymbolInformation>(json, Serializer.Instance.Options);
            result.Kind.Should().Be(SymbolKind.Event);
        }

        [Fact]
        public void DefaultBehavior_Should_Only_Support_InitialSymbolTags()
        {

            var json = JsonSerializer.Serialize(new SymbolInformation()
            {
                Tags = new Container<SymbolTag>(SymbolTag.Deprecated)
            }, Serializer.Instance.Options);

            var result = JsonSerializer.Deserialize<SymbolInformation>(json, Serializer.Instance.Options);
            result.Tags.Should().Contain(SymbolTag.Deprecated);
        }

        [Fact]
        public void CustomBehavior_When_SymbolKind_Defined_By_Client()
        {
            var serializer = new Serializer();
            serializer.SetClientCapabilities(ClientVersion.Lsp3, new ClientCapabilities()
            {
                Workspace = new WorkspaceClientCapabilities()
                {
                    Symbol = new Supports<WorkspaceSymbolCapability>(true, new WorkspaceSymbolCapability()
                    {
                        DynamicRegistration = true,
                        SymbolKind = new SymbolKindCapability()
                        {
                            ValueSet = new Container<SymbolKind>(SymbolKind.Class)
                        }
                    })
                }
            });
            var json = JsonSerializer.Serialize(new SymbolInformation()
            {
                Kind = SymbolKind.Event
            }, serializer.Options);

            var result = JsonSerializer.Deserialize<SymbolInformation>(json, serializer.Options);
            result.Kind.Should().Be(SymbolKind.Class);
        }

        [Fact]
        public void CustomBehavior_When_SymbolTag_Defined_By_Client()
        {

            var serializer = new Serializer();
            serializer.SetClientCapabilities(ClientVersion.Lsp3, new ClientCapabilities()
            {
                Workspace = new WorkspaceClientCapabilities()
                {
                    Symbol = new Supports<WorkspaceSymbolCapability>(true, new WorkspaceSymbolCapability()
                    {
                        DynamicRegistration = true,
                        TagSupport = new TagSupportCapability() {
                            ValueSet = new Container<SymbolTag>()
                        }
                    })
                }
            });

            var json = JsonSerializer.Serialize(new SymbolInformation()
            {
                Tags = new Container<SymbolTag>(SymbolTag.Deprecated)
            }, serializer.Options);

            var result = JsonSerializer.Deserialize<SymbolInformation>(json, serializer.Options);
            result.Tags.Should().BeEmpty();
        }
    }
}