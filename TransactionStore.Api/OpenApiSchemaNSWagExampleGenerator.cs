using Namotion.Reflection;
using NJsonSchema.Generation;
using NSwag.Generation;

namespace TransactionStore.Api
{
    public class OpenApiSchemaNSWagExampleGenerator : OpenApiSchemaGenerator
    {
        /// <inheritdoc />
        public OpenApiSchemaNSWagExampleGenerator(JsonSchemaGeneratorSettings settings)
            : base(settings)
        {
        }

        public override object? GenerateExample(ContextualType type)
        {
            if (!Settings.GenerateExamples)
                return null;

            try
            {
                var docs = type is ContextualMemberInfo member ? member.GetXmlDocsTag("example") : type.GetXmlDocsTag("example");

                return !string.IsNullOrEmpty(docs) ? docs : null;
            }
            catch
            {
                return null;
            }
        }
    }
}