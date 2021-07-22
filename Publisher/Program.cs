using RabbitMQ.Client;
using System;
using System.Text;

namespace Publisher
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Configurando a conexão
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = ConnectionFactory.DefaultUser,
                Password = ConnectionFactory.DefaultPass
            };

            // Criando a conexão e o canal onde é definido a fila com sua mensagem e sua publicação
            // O canal existe no contexto da conexão, caso a conexão seja fechada, será fechado também todos os canais
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                /*  Declarando uma fila:
                 *      queue: nome da fila
                 *      durable: caso true, fila permanece ativa após servidor ser reiniciado
                 *      exclusive: caso true, fila só poder ser acessada na conexão atual e é excluída ao fechar a conexão
                 *      autoDelete: caso true, fila será deletada automaticamente após ser consumida
                 */
                channel.QueueDeclare(
                    queue: "ola_mundo",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var message = "Testando RabbitMQ";
                var body = Encoding.UTF8.GetBytes(message);

                // Publicando a fila
                channel.BasicPublish(
                    exchange: String.Empty,
                    routingKey: "ola_mundo",
                    basicProperties: null,
                    body: body);
            }
        }
    }
}
