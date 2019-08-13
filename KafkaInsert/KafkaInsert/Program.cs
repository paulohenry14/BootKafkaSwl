using System;
using System.Data.SqlClient;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace KafkaInsert
{
    public class Program
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig();

            config.BootstrapServers = "23.99.218.43:9092";
            config.GroupId = "Grupo1";

            var builder = new ConsumerBuilder<string, string>(config);

            using (var consumer = builder.Build())
            {
                Console.Write("Ouvindo Kafka");
                consumer.Subscribe("15netkafka");

                while (true)
                {
                    var result = consumer.Consume(TimeSpan.FromSeconds(1));

                    if (result != null && result.Value != null)
                    {
                        PerguntaResposta perguntaResposta = JsonConvert.DeserializeObject < PerguntaResposta > (result.Value);

                        Console.WriteLine("Pergunta: " + perguntaResposta.Pergunta, "Resposta: " + perguntaResposta.Resposta);

                        InsertSQL(perguntaResposta.Pergunta, perguntaResposta.Resposta);

                    }
                }
            }

        }

        public static SqlConnection getConnection()
        {
            string connectionString = "Data Source=saturnoserver.database.windows.net;User=user;Password=fiap-net15;database=FIAP";
            SqlConnection sqlConn = new SqlConnection(connectionString);

            return sqlConn;
        }

        public static void InsertSQL(string pergunta, string resposta)
        {
            string query = "INSERT INTO PERGUNTASRESPOSTAS (Pergunta, Resposta) VALUES " + " ('" + pergunta + "'"+ "," + "'" + resposta + "')" ;

            using (SqlConnection sqlConnection = getConnection())
            {
                SqlCommand command = new SqlCommand(query, sqlConnection);

                command.Connection.Open();

                try
                {
                    command.ExecuteNonQuery();
                } catch(Exception ex)
                {
                }

                sqlConnection.Close();
            }
        }
    }

}
