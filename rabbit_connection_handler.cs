using System.Text;
using RabbitMQ.Client;


namespace Streamer
{
    class RabbitConnectionHandler
    {
        private ConnectionFactory factory;
        public ConnectionFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }
        private IConnection connection;
        public IConnection Connection
        {
            get { return connection; }
            set { connection = value; }
        }

        private IModel channel;
        public IModel Channel
        {
            get { return channel; }
            set { channel = value; }
        }


        public RabbitConnectionHandler()
        {
            this.Factory = new ConnectionFactory { HostName = "localhost" };
            this.Connection = factory.CreateConnection();
            this.Channel = connection.CreateModel();
        }

        ~RabbitConnectionHandler()
        {
            //Need to add a method fore disposing the connection
        }
    }
}