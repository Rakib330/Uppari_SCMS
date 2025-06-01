using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Numerics;
using System.Text.Json;
using Uppari_SCMS.Data.Models;

namespace Uppari_SCMS.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        //Default Entitys
        public DbSet<AuditLog> AuditLogs { get; set; }

        public DbSet<Item> Items { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AuditLog>().HasKey(a => a.AuditLogId);
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Vendor>().ToTable("Vendors");
            modelBuilder.Entity<PurchaseOrder>().ToTable("PurchaseOrders");
        }

        public override int SaveChanges()
        {
            ApplyAuditInfo();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }
        private void ApplyAuditInfo()
        {
            var userId = GetCurrentUserId();
            var auditLogs = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries<BaseModel>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.EntryTime = DateTime.Now;
                    entry.Entity.EntryBy = userId;
                }

                if (entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    entry.Entity.ModifiedTime = DateTime.Now;
                    entry.Entity.ModifiedBy = userId;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
                {
                    var latitude = 23.7809;
                    var longitude = 90.4071;
                    var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                    var location = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
                    var audit = new AuditLog
                    {
                        TableName = entry.Entity.GetType().Name,
                        KeyValues = GetPrimaryKey(entry),
                        ActionType = entry.State.ToString(),
                        UserId = userId,
                        Location = location,
                        ActionDate = DateTime.Now
                    };

                    if (entry.State == EntityState.Modified)
                    {
                        audit.OldValues = GetPropertyValues(entry.OriginalValues.Properties, entry.OriginalValues);
                        audit.NewValues = GetPropertyValues(entry.CurrentValues.Properties, entry.CurrentValues);
                    }
                    else if (entry.State == EntityState.Deleted)
                    {
                        audit.OldValues = GetPropertyValues(entry.OriginalValues.Properties, entry.OriginalValues);
                    }
                    else if (entry.State == EntityState.Added)
                    {
                        audit.NewValues = GetPropertyValues(entry.CurrentValues.Properties, entry.CurrentValues);
                    }
                    auditLogs.Add(audit);
                }
            }
            if (auditLogs.Any())
            {
                AuditLogs.AddRange(auditLogs);
            }
        }
        private int GetCurrentUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            return int.TryParse(userIdStr, out var userId) ? userId : 0;
        }
        private string GetPrimaryKey(EntityEntry entry)
        {
            var pk = entry.Properties
                .Where(p => p.Metadata.IsPrimaryKey())
                .Select(p => $"{p.Metadata.Name}={p.CurrentValue}")
                .ToList();

            return string.Join(",", pk);
        }
        private string GetPropertyValues(IEnumerable<IProperty> properties, PropertyValues values)
        {
            var dict = new Dictionary<string, object?>();

            foreach (var prop in properties)
            {
                dict[prop.Name] = values[prop.Name];
            }

            return JsonSerializer.Serialize(dict); // System.Text.Json
        }
    }
}
