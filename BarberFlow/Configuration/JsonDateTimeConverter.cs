using System.Text.Json;

namespace BarberFlow.API.Configuration
{
    public class JsonDateTimeConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string input = reader.GetString();

            // 1. Checagem de Nulo ou Vazio
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new JsonException("A data não pode ser nula ou vazia.");
            }

            // 2. Tenta converter o formato completo (ISO 8601)
            if (DateTime.TryParse(input, out DateTime result))
            {
                return result.ToUniversalTime();
            }

            // 3. Tenta converter se mandarem apenas a hora (Opcional, mas ajuda no teste)
            if (TimeSpan.TryParse(input, out TimeSpan time))
            {
                return DateTime.Today.Add(time).ToUniversalTime();
            }

            throw new JsonException($"O formato '{input}' não é uma data válida. Use o padrão yyyy-MM-ddTHH:mm:ss");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Quando envia para o Front-end, envia com o "Z" de Zulu (UTC)
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ"));
        }
    };
}
