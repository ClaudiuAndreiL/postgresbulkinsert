using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace GeneratedCode
{
    [Generator]
    public class DatabaseModelSerializerGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            // Retrieve the populated receiver
            if (context.SyntaxReceiver is not SyntaxReceiver receiver)
                return;

            // Get the semantic model
            var compilation = context.Compilation;

            foreach (var classDeclaration in receiver.CandidateClasses)
            {
                var model = compilation.GetSemanticModel(classDeclaration.SyntaxTree);
                var classSymbol = model.GetDeclaredSymbol(classDeclaration) as INamedTypeSymbol;

                if (classSymbol == null)
                    continue;

                // Check if the class has the specified attribute
                var hasGenerateSerializerAttribute = classSymbol.GetAttributes()
                    .Any(ad => ad.AttributeClass?.Name == typeof(GenerateSerializerAttribute).Name);

                if (!hasGenerateSerializerAttribute)
                    continue;

                var source = GenerateSerializerCode(classSymbol);
                context.AddSource($"{classSymbol.Name}NpgsqlBinarySerializer.g.cs", SourceText.From(source, Encoding.UTF8));
            }
        }

        private static string GenerateSerializerCode(INamedTypeSymbol classSymbol)
        {
            // Debugger.Launch();

            var properties = classSymbol.GetMembers().OfType<IPropertySymbol>();
            var sb = new StringBuilder();

            sb.AppendLine($@"
using Common;
using Npgsql;
using NpgsqlTypes;

namespace {classSymbol.ContainingNamespace}
{{
    public class {classSymbol.Name}NpgsqlBinarySerializer : INpgsqlEntityBinarySerializer<{classSymbol.Name}>
    {{
        public async Task BulkInsertBinaryAsync(NpgsqlBinaryImporter writer, NpgsqlConnection connection, {classSymbol.Name} message)
        {{
            await writer.StartRowAsync();
            ");

            //TODO: eliminate auto-generated properties in a dynamic way
            foreach (var property in properties.Where(p => !p.Name.Contains("OrderNo")))
            {
                sb.AppendLine($@"
await writer.WriteAsync(message.{property.Name}, {GetNpgsqlDbType(property.Type.ToString())});
            ");
            }

            sb.AppendLine(@"
                    }
                }
            }
            ");

            return sb.ToString();
        }

        //TODO: improve using the already existing dictionary
        public static string GetNpgsqlDbType(string typeName)
        {
            if (typeName == "System.Guid" || typeName == "System.Guid?")
                return "NpgsqlDbType.Uuid";
            else if (typeName == "string" || typeName == "string?")
                return "NpgsqlDbType.Citext";
            else if (typeName == "bool" || typeName == "bool?")
                return "NpgsqlDbType.Boolean";
            else if (typeName == "int")
                return "NpgsqlDbType.Integer";
            else if (typeName == "System.DateTime" || typeName == "System.DateTime?")
                return "NpgsqlDbType.TimestampTz";
            else
                throw new NotSupportedException($"Type {typeName} is not supported.");
        }

        class SyntaxReceiver : ISyntaxReceiver
        {
            public List<ClassDeclarationSyntax> CandidateClasses { get; } = new List<ClassDeclarationSyntax>();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
            {
                if (syntaxNode is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    CandidateClasses.Add(classDeclarationSyntax);
                }
            }
        }
    }
}
