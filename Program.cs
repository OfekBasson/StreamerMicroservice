namespace Streamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();


            DataStreamer ds = new DataStreamer();
            using (RabbitConnectionHandler rch = new RabbitConnectionHandler())
            {
                app.MapGet("/", () =>
                {
                    try
                    {
                        string id = ds.StreamDataAndReturnImageId();
                        rch.PublishID(id);
                        return Results.Ok("Success"); 
                    }
                    catch (Exception e)
                    {
                        return Results.BadRequest(e.Message);
                    }

                }).WithOpenApi(operation => new(operation)
                {
                    Summary = "Activating streamer",
                    Description = "Copying data directories from 'data_origin' directory to 'data_destination' directory. Data directory includes an image and a json file with metadata."
                });

                app.Run();
            }
        }
    }
}