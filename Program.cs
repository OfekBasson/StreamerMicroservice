namespace Streamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            DataStreamer ds = new DataStreamer();
            using (RabbitConnectionHandler rch = new RabbitConnectionHandler())
            {
                app.MapGet("/", () =>
                {
                    try
                    {
                        string id = ds.StreamDataAndReturnImageId();
                        rch.PublishID(id);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                });

                app.Run();
            }
        }
    }
}