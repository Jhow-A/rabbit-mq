using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Consumer
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
                channel.QueueDeclare(
                    queue: "ola_mundo",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                // Solicitando a entrega das mensagens de forma assíncrona
                var consumer = new EventingBasicConsumer(channel);

                // Recebendo a mensagem
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Mensagem recebida: {message}");
                };

                // Indica o consume da mensagem no RabbitMQ
                channel.BasicConsume(
                    queue: "ola_mundo",
                    autoAck: true,
                    consumer: consumer);
            }

            Console.ReadKey();
        }
    }
}
