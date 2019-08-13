using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaInsert
{
    public class PerguntaResposta
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("pergunta")]
        public string Pergunta { get; set; }
        [JsonProperty("resposta")]
        public string Resposta { get; set; }
    }
}
