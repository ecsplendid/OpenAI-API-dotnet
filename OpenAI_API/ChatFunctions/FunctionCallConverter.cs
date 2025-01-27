﻿using System;
using Newtonsoft.Json;
namespace OpenAI_API.ChatFunctions
{
    internal class FunctionCallConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(FunctionCall));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var functionCall = value as FunctionCall;

            if (functionCall.Name == "none" || functionCall.Name == "auto")
            {
                serializer.Serialize(writer, functionCall.Name);
            }
            else
            {
                serializer.Serialize(writer, functionCall);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var functionCallType = (string)serializer.Deserialize(reader, typeof(string));

                if (functionCallType == "none" || functionCallType == "auto")
                {
                    return new FunctionCall { Name = functionCallType };
                }
            }
            else if (reader.TokenType == JsonToken.StartObject)
            {
                return serializer.Deserialize<FunctionCall>(reader);
            }

            throw new ArgumentException("Unsupported type for FunctionCall");
        }
    }

}
