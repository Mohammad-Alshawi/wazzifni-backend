﻿using Microsoft.OpenApi.Any;
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
            schema.Enum = Enum.GetNames(context.Type)
                   .Select(name => (IOpenApiAny)new OpenApiString(name))
                   .ToList();
            schema.Type = "string";
        }
    }
}


