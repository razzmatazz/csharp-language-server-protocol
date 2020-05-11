using System.Text.Json;
using FluentAssertions;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Serialization;
using Xunit;

namespace Lsp.Tests
{
    public class CompletionItemKindTests
    {
        [Fact]
        public void DefaultBehavior_Should_Only_Support_InitialCompletionItemKinds()
        {
            var serializer = new Serializer();

            var json = JsonSerializer.Serialize(new CompletionItem() {
                Kind = CompletionItemKind.Event
            }, serializer.Options);

            var result = JsonSerializer.Deserialize<CompletionItem>(json, serializer.Options);
            result.Kind.Should().Be(CompletionItemKind.Event);
        }

        [Fact]
        public void DefaultBehavior_Should_Only_Support_InitialCompletionItemTags()
        {
            var serializer = new Serializer();

            var json = JsonSerializer.Serialize(new CompletionItem() {
                Tags = new Container<CompletionItemTag>(CompletionItemTag.Deprecated)
            }, serializer.Options);

            var result = JsonSerializer.Deserialize<CompletionItem>(json, serializer.Options);
            result.Tags.Should().Contain(CompletionItemTag.Deprecated);
        }

        [Fact]
        public void CustomBehavior_When_CompletionItemKinds_Defined_By_Client()
        {
            var serializer = new Serializer();
            serializer.SetClientCapabilities(ClientVersion.Lsp3, new ClientCapabilities() {
                TextDocument = new TextDocumentClientCapabilities {
                    Completion = new Supports<CompletionCapability>(true, new CompletionCapability() {
                        DynamicRegistration = true,
                        CompletionItemKind = new CompletionItemKindCapability() {
                            ValueSet = new Container<CompletionItemKind>(CompletionItemKind.Class)
                        }
                    })
                }
            });

            var json = JsonSerializer.Serialize(new CompletionItem() {
                Kind = CompletionItemKind.Event
            }, serializer.Options);

            var result = JsonSerializer.Deserialize<CompletionItem>(json, serializer.Options);
            result.Kind.Should().Be(CompletionItemKind.Class);
        }

        [Fact]
        public void CustomBehavior_When_CompletionItemTags_Defined_By_Client()
        {
            var serializer = new Serializer();
            serializer.SetClientCapabilities(ClientVersion.Lsp3, new ClientCapabilities() {
                TextDocument = new TextDocumentClientCapabilities {
                    Completion = new Supports<CompletionCapability>(true, new CompletionCapability() {
                        DynamicRegistration = true,
                        CompletionItem = new CompletionItemCapability() {
                            TagSupport = new CompletionItemTagSupportCapability() {
                                ValueSet = new Container<CompletionItemTag>()
                            }
                        }
                    })
                }
            });

            var json = JsonSerializer.Serialize(new CompletionItem() {
                Tags = new Container<CompletionItemTag>(CompletionItemTag.Deprecated)
            }, serializer.Options);

            var result = JsonSerializer.Deserialize<CompletionItem>(json, serializer.Options);
            result.Tags.Should().BeEmpty();
        }
    }
}

