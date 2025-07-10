using Microsoft.EntityFrameworkCore;
using TeduBlog.Data;

namespace TeduBlog.Api
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<TeduBlogContext>())
                {
                    // auto migate db when add-migration, don't need to update-database
                    context.Database.Migrate();
                    new DataSeeder().SeedAsync(context).Wait();
                }
            }
            return app;
        }
    }
}
