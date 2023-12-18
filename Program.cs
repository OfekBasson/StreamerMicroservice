namespace Streamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            DataStreamer ds = new DataStreamer();

            app.MapGet("/", () => ds.StreamData());

            app.Run();
        }
    }
}