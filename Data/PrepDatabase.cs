using Microsoft.EntityFrameworkCore;

namespace RIA.API.Data;

public static class PrepDatabase
{
    public static void PrepareDatabase(IApplicationBuilder app, IWebHostEnvironment env)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        MigrateDatabase(dbContext, env);
    }

    private static void MigrateDatabase(AppDbContext dbContext, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            try
            {
                Console.WriteLine("--> Attempting to migrate database...");
                dbContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Failed to apply database migrations. Reason: {e.Message}");
            }
        }
    }
}