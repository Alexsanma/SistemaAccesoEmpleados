using AccessControl.Domain.AccessPoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AccessControl.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AccessControlDbContext>();

        await context.Database.MigrateAsync();
        await SeedAccessPointsAsync(context);
    }

    private static async Task SeedAccessPointsAsync(AccessControlDbContext context)
    {
        if (await context.AccessPoints.AnyAsync())
            return;

        context.AccessPoints.AddRange(
            AccessPoint.Create("Recepción principal", AccessPointType.Reception, "Sede principal - primer piso"),
            AccessPoint.Create("Parqueadero", AccessPointType.Parking, "Sede principal - sótano"),
            AccessPoint.Create("Bodega central", AccessPointType.Warehouse, "Bloque de almacenamiento"),
            AccessPoint.Create("Cuarto de servidores", AccessPointType.RestrictedArea, "Sede principal - tercer piso"));

        await context.SaveChangesAsync();
    }
}
