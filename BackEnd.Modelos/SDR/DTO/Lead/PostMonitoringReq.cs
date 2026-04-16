using System.Reflection.Metadata;
using System.Text.Json.Serialization;

namespace BackEnd.Modelos.SDR.DTO.Lead
{
    public record PostMonitoringReq([property: JsonPropertyName("data_acompanhamento")] DateTime MontoringDate);
}
