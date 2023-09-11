﻿using CmsKitDemo.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace CmsKitDemo.Data;

public class CmsKitDemoDbContext : AbpDbContext<CmsKitDemoDbContext>
{
    public DbSet<GalleryImage> GalleryImages { get; set; }
    
    public CmsKitDemoDbContext(DbContextOptions<CmsKitDemoDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();
        builder.ConfigureCmsKit();

        /* Configure your own entities here */
        builder.Entity<GalleryImage>(b =>
        {
            b.ToTable(CmsKitDemoConsts.DbTablePrefix + "Images", CmsKitDemoConsts.DbSchema);
            b.ConfigureByConvention();
        });
    }
}
