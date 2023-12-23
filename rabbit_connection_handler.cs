using System.Text;
using RabbitMQ.Client;

namespace Streamer
{
    class RabbitConnectionHandler : IDisposable
    {
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;
        private bool disposed = false;

        public RabbitConnectionHandler()
        {
            factory = new ConnectionFactory { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "hello",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        }

        public void PublishID(string id)
        {
            byte[] body = Encoding.UTF8.GetBytes(id);
            channel.BasicPublish(exchange: string.Empty,
                     routingKey: "hello",
                     basicProperties: null,
                     body: body);
            Console.WriteLine($" [x] Sent {id}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    channel?.Dispose();
                    connection?.Dispose();
                }

                disposed = true;
            }
        }

        ~RabbitConnectionHandler()
        {
            Dispose(disposing: false);
        }
    }
}