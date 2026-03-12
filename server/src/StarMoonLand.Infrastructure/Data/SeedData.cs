using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StarMoonLand.Core.Entities;

namespace StarMoonLand.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 自動 Migration
        await context.Database.MigrateAsync();

        // 建立角色
        var roles = new[] { "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // 建立預設管理員
        const string adminEmail = "admin@starmoonland.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                DisplayName = "系統管理員",
                EmailConfirmed = true,
                IsActive = true
            };
            await userManager.CreateAsync(admin, "Admin@123");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // 建立頁面分類
        if (!await context.PageCategories.AnyAsync())
        {
            context.PageCategories.AddRange(
                new PageCategory { Slug = "about", TitleZh = "星月故事", TitleEn = "About", SortOrder = 1 },
                new PageCategory { Slug = "service", TitleZh = "星月服務", TitleEn = "Service", SortOrder = 2 },
                new PageCategory { Slug = "dining", TitleZh = "餐飲宴會", TitleEn = "Dining", SortOrder = 3 },
                new PageCategory { Slug = "wedding", TitleZh = "幸福婚宴", TitleEn = "Wedding", SortOrder = 4 }
            );
            await context.SaveChangesAsync();
        }

        // 建立子頁面
        if (!await context.Pages.AnyAsync())
        {
            var about = await context.PageCategories.FirstAsync(c => c.Slug == "about");
            var service = await context.PageCategories.FirstAsync(c => c.Slug == "service");
            var dining = await context.PageCategories.FirstAsync(c => c.Slug == "dining");
            var wedding = await context.PageCategories.FirstAsync(c => c.Slug == "wedding");

            context.Pages.AddRange(
                // 星月故事
                new Page { CategoryId = about.Id, Slug = "origin", Title = "星月緣起", SortOrder = 1 },
                new Page { CategoryId = about.Id, Slug = "vision", Title = "星月願景", SortOrder = 2 },
                new Page { CategoryId = about.Id, Slug = "mascot", Title = "吉祥物介紹", SortOrder = 3 },
                // 星月服務
                new Page { CategoryId = service.Id, Slug = "camp", Title = "大地營區", SortOrder = 1 },
                new Page { CategoryId = service.Id, Slug = "bbq", Title = "大地烤肉", SortOrder = 2 },
                new Page { CategoryId = service.Id, Slug = "hotspring", Title = "大地風呂", SortOrder = 3 },
                new Page { CategoryId = service.Id, Slug = "stay", Title = "星月文旅", SortOrder = 4 },
                // 餐飲宴會
                new Page { CategoryId = dining.Id, Slug = "banquet", Title = "景觀宴會館", SortOrder = 1 },
                new Page { CategoryId = dining.Id, Slug = "cafe", Title = "景觀咖啡廳", SortOrder = 2 },
                // 幸福婚宴
                new Page { CategoryId = wedding.Id, Slug = "proposal", Title = "求婚策畫", SortOrder = 1 },
                new Page { CategoryId = wedding.Id, Slug = "ceremony", Title = "婚禮席宴", SortOrder = 2 }
            );
            await context.SaveChangesAsync();
        }

        // 建立新聞分類
        if (!await context.NewsCategories.AnyAsync())
        {
            context.NewsCategories.AddRange(
                new NewsCategory { Name = "活動情報", Slug = "events", SortOrder = 1 },
                new NewsCategory { Name = "媒體報導", Slug = "media", SortOrder = 2 },
                new NewsCategory { Name = "其他公告", Slug = "others", SortOrder = 3 }
            );
            await context.SaveChangesAsync();
        }

        // 建立網站設定
        if (!await context.SiteSettings.AnyAsync())
        {
            context.SiteSettings.AddRange(
                new SiteSetting { Key = "site_name", Value = "星月大地景觀休閒園區", Description = "網站名稱" },
                new SiteSetting { Key = "phone", Value = "04-25831839", Description = "聯絡電話" },
                new SiteSetting { Key = "address", Value = "台中市后里區月眉北路482號", Description = "地址" },
                new SiteSetting { Key = "email", Value = "", Description = "聯絡信箱" },
                new SiteSetting { Key = "facebook_url", Value = "", Description = "Facebook 連結" },
                new SiteSetting { Key = "line_url", Value = "", Description = "LINE 連結" },
                new SiteSetting { Key = "instagram_url", Value = "", Description = "Instagram 連結" },
                new SiteSetting { Key = "google_maps_embed", Value = "", Description = "Google Maps 嵌入 URL" },
                new SiteSetting { Key = "admin_notification_email", Value = "", Description = "管理者通知信箱" },
                new SiteSetting { Key = "footer_text", Value = "© 星月大地景觀休閒園區 All Rights Reserved.", Description = "頁腳文字" }
            );
            await context.SaveChangesAsync();
        }

        // 建立交通資訊
        if (!await context.TrafficInfos.AnyAsync())
        {
            context.TrafficInfos.AddRange(
                new TrafficInfo { TabName = "市區公車", Content = "<p>搭乘豐原客運至月眉站下車</p>", SortOrder = 1 },
                new TrafficInfo { TabName = "自行開車", Content = "<p>國道一號后里交流道下，往月眉方向約10分鐘</p>", SortOrder = 2 }
            );
            await context.SaveChangesAsync();
        }
    }
}
