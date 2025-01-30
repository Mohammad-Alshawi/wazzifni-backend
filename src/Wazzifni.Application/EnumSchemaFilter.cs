using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum = Enum.GetValues(context.Type)
               .Cast<Enum>()
               .Select(e => new Microsoft.OpenApi.Any.OpenApiInteger(Convert.ToInt32(e)))
               .ToList<Microsoft.OpenApi.Any.IOpenApiAny>();
            schema.Type = "integer";
            schema.Format = null;
        }
    }
}


