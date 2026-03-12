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
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

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

        // === 頁面分類 ===
        await SeedOrUpdateCategoryAsync(context, "about", "星月故事", "About", "/_images/about/banner.png", 1);
        await SeedOrUpdateCategoryAsync(context, "service", "星月服務", "Service", "/_images/service/banner.png", 2);
        await SeedOrUpdateCategoryAsync(context, "dining", "餐飲宴會", "Dining", "/_images/dining/banner.png", 3);
        await SeedOrUpdateCategoryAsync(context, "wedding", "幸福婚宴", "Wedding", "/_images/wedding/banner.png", 4);
        await context.SaveChangesAsync();

        var aboutId = (await context.PageCategories.FirstAsync(c => c.Slug == "about")).Id;
        var serviceId = (await context.PageCategories.FirstAsync(c => c.Slug == "service")).Id;
        var diningId = (await context.PageCategories.FirstAsync(c => c.Slug == "dining")).Id;
        var weddingId = (await context.PageCategories.FirstAsync(c => c.Slug == "wedding")).Id;

        // === 頁面內容 ===
        // 星月故事
        await SeedOrUpdatePageAsync(context, aboutId, "origin", "星月緣起", null, 1,
            @"<div class=""text-center"">
<p>像每個人一樣<br>為自己、家人、朋友、最在乎的人<br>留了一個重要的位置</p>
<p>可以放肆地哭<br>但我們希望是盡情地笑多一點<br>可以脫下長戴的面具<br>但我們希望是溫暖的笑容多一些</p>
<p>這裡是星月大地<br>與大家一同分享心生活的星之所</p>
</div>");

        await SeedOrUpdatePageAsync(context, aboutId, "vision", "星月願景", null, 2,
            @"<p>我們是星月大地，矗立於后里及外埔交界最高河階面上的景觀休閒園區，我們有你生活中不可或缺的美好，走進園區，你(妳)可以在這裡開始你與她(他)的故事、你可以開始你與家人們的生活篇章、你可以開始你與同事、朋友們生活中最重要的小事；全世界最大的生意，就是「分享」，我們願意與您共享園區的所有，你們願意與我們分享你妳們的笑容與故事嗎？</p>");

        await SeedOrUpdatePageAsync(context, aboutId, "mascot", "吉祥物介紹", null, 3,
            @"<div class=""grid md:grid-cols-2 gap-8"">
<div>
<img src=""/_images/about/Shiny.png"" alt=""小鹿Shiny"" class=""w-full max-w-sm mx-auto mb-4"" />
<h3 class=""text-lg font-bold mb-2"">小鹿Shiny</h3>
<p>嗨，我是不鳴則已，一鳴驚人的Shiny！最喜歡大家叫我帥尼。我有一些大頭症，只欣賞所有「美」的事物，例如我們家壯闊的美景、跟我一樣瞎趴閃亮亮的夜景還有園區千萬造景；另外，我也很會策畫及主持大小活動，例如最近超夯的「戶外求婚與證婚」，並記錄活動大小事與大家分享，歡迎大家常常過來看我與Sunny喲!</p>
</div>
<div>
<img src=""/_images/about/Sunny.png"" alt=""小鹿Sunny"" class=""w-full max-w-sm mx-auto mb-4"" />
<h3 class=""text-lg font-bold mb-2"">小鹿Sunny</h3>
<p>哈嘍，我是識(挑)貨(食)的愛吃鬼Sunny，有些叔叔阿姨會直接叫我桑妮或妮妮。我最愛吃甜食，而且只吃好吃的東西，例如我們家的蜜糖吐司還有大地冰紛樂，當然，愛煮東西的我，也只分享正港好吃、健康的給大家啦。歡迎大家常常帶食物來看我(誤)，我會努力記錄所有美食，並與大家分享的！啾咪～</p>
</div>
</div>");

        // 星月服務
        await SeedOrUpdatePageAsync(context, serviceId, "camp", "大地營區", "遠眺美景．星月大地．親子野餐", 1, null);
        await SeedOrUpdatePageAsync(context, serviceId, "bbq", "大地烤肉", "星月大地提供大台中微涼夜烤的新選擇", 2, null);
        await SeedOrUpdatePageAsync(context, serviceId, "hotspring", "大地風呂", "露天冷熱湯池、多功能SPA池、檜木烤箱、蒸氣室、五星級衛浴", 3, null);
        await SeedOrUpdatePageAsync(context, serviceId, "stay", "星月文旅", "旅途中放慢腳步、放鬆身心、享受在星月大地的「美」分「美」秒", 4, null);

        // 餐飲宴會
        await SeedOrUpdatePageAsync(context, diningId, "banquet", "景觀宴會館", "愛家人的心做料理", 1, null);
        await SeedOrUpdatePageAsync(context, diningId, "cafe", "景觀咖啡廳", "270度無死角觀景台", 2, null);

        // 幸福婚宴
        await SeedOrUpdatePageAsync(context, weddingId, "proposal", "求婚策畫", "永遠不散的戀愛", 1, null);
        await SeedOrUpdatePageAsync(context, weddingId, "ceremony", "婚禮席宴", "親愛的，這就是愛情", 2, null);

        await context.SaveChangesAsync();

        // === 頁面輪播圖 ===
        await SeedPageSlidesIfEmptyAsync(context);

        // === 頁面 Tab ===
        await SeedPageTabsIfEmptyAsync(context);

        // === 新聞分類 ===
        await SeedOrUpdateNewsCategoryAsync(context, "events", "活動情報", 1);
        await SeedOrUpdateNewsCategoryAsync(context, "media", "媒體報導", 2);
        await SeedOrUpdateNewsCategoryAsync(context, "others", "其他公告", 3);
        await context.SaveChangesAsync();

        // === 範例新聞 ===
        await SeedNewsIfEmptyAsync(context);

        // === 相簿 ===
        await SeedAlbumsIfEmptyAsync(context);

        // === 首頁輪播 ===
        await SeedHomepageSlidesAsync(context);

        // === 網站設定 ===
        await SeedOrUpdateSettingAsync(context, "site_name", "星月大地景觀休閒園區", "網站名稱");
        await SeedOrUpdateSettingAsync(context, "phone", "04-26831671", "聯絡電話");
        await SeedOrUpdateSettingAsync(context, "fax", "04-26834003", "傳真號碼");
        await SeedOrUpdateSettingAsync(context, "address", "台中市后里區月眉北路486號(大摩天輪正後方)", "地址");
        await SeedOrUpdateSettingAsync(context, "email", "", "聯絡信箱");
        await SeedOrUpdateSettingAsync(context, "facebook_url", "", "Facebook 連結");
        await SeedOrUpdateSettingAsync(context, "line_url", "", "LINE 連結");
        await SeedOrUpdateSettingAsync(context, "instagram_url", "", "Instagram 連結");
        await SeedOrUpdateSettingAsync(context, "google_maps_embed",
            "https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d14541.597505171469!2d120.701191!3d24.332577!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0xf22f690b198e3e44!2z5pif5pyI5aSn5Zyw5LyR6ZaS5LqL5qWt5pyJ6ZmQ5YWs5Y-4!5e0!3m2!1sen!2shk!4v1465960095168",
            "Google Maps 嵌入 URL");
        await SeedOrUpdateSettingAsync(context, "admin_notification_email", "", "管理者通知信箱");
        await SeedOrUpdateSettingAsync(context, "footer_text", "COPYRIGHT © STAR MOON LAND ALL RIGHTS RESERVED.", "頁腳文字");
        await context.SaveChangesAsync();

        // === 交通資訊 ===
        await SeedOrUpdateTrafficAsync(context);

        // === 修正舊的圖片路徑前綴 ===
        await FixImageUrlPrefixesAsync(context);
    }

    // ===== Helper Methods =====

    private static async Task SeedOrUpdateCategoryAsync(AppDbContext ctx, string slug, string titleZh, string titleEn, string? bannerImage, int sortOrder)
    {
        var cat = await ctx.PageCategories.FirstOrDefaultAsync(c => c.Slug == slug);
        if (cat == null)
        {
            ctx.PageCategories.Add(new PageCategory { Slug = slug, TitleZh = titleZh, TitleEn = titleEn, BannerImage = bannerImage, SortOrder = sortOrder });
        }
        else
        {
            cat.BannerImage ??= bannerImage;
        }
    }

    private static async Task SeedOrUpdatePageAsync(AppDbContext ctx, int categoryId, string slug, string title, string? subtitle, int sortOrder, string? content)
    {
        var page = await ctx.Pages.FirstOrDefaultAsync(p => p.Slug == slug && p.CategoryId == categoryId);
        if (page == null)
        {
            ctx.Pages.Add(new Page { CategoryId = categoryId, Slug = slug, Title = title, Subtitle = subtitle, Content = content, SortOrder = sortOrder });
        }
        else
        {
            // 更新空白內容
            if (string.IsNullOrEmpty(page.Content) && !string.IsNullOrEmpty(content))
                page.Content = content;
            if (string.IsNullOrEmpty(page.Subtitle) && !string.IsNullOrEmpty(subtitle))
                page.Subtitle = subtitle;
        }
    }

    private static async Task SeedPageSlidesIfEmptyAsync(AppDbContext ctx)
    {
        // 修正舊的圖片路徑（不含 /_images/ 前綴的）
        var oldSlides = await ctx.Set<PageSlide>().Where(s => !s.ImageUrl.StartsWith("/")).ToListAsync();
        foreach (var s in oldSlides) s.ImageUrl = $"/_images/{s.ImageUrl}";
        if (oldSlides.Count > 0) await ctx.SaveChangesAsync();

        if (await ctx.Set<PageSlide>().AnyAsync()) return;

        var pages = await ctx.Pages.Include(p => p.Category).ToListAsync();
        Page? GetPage(string catSlug, string pageSlug) => pages.FirstOrDefault(p => p.Category.Slug == catSlug && p.Slug == pageSlug);

        var vision = GetPage("about", "vision");
        var camp = GetPage("service", "camp");
        var bbq = GetPage("service", "bbq");
        var hotspring = GetPage("service", "hotspring");
        var stay = GetPage("service", "stay");
        var banquet = GetPage("dining", "banquet");
        var cafe = GetPage("dining", "cafe");
        var proposal = GetPage("wedding", "proposal");
        var ceremony = GetPage("wedding", "ceremony");

        var slides = new List<PageSlide>();

        if (vision != null)
        {
            slides.Add(new PageSlide { PageId = vision.Id, ImageUrl = "/_images/about/class-1-pic-1.png", Title = "壯麗山景", Description = "隨著四季、氣候不同的轉變，有壯麗清新的山景、落日餘暉的火燒雲夕陽、萬家燈火及滿天繁星的熠熠夜景、可遇不可求如仙境般的漫漫雲海，以及站上這片大地的制高點，不必登高山，即可瞭望遠方山海空一線美景。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = vision.Id, ImageUrl = "/_images/about/class-1-pic-2.png", Title = "南洋情調", Description = "走在園區造景，不必出國，就能感受濃厚南洋般的異國情調、美饌上桌，多元的餐飲服務選擇，滿足不同饕客的絕對味蕾。", SortOrder = 2 });
        }

        if (camp != null)
        {
            slides.Add(new PageSlide { PageId = camp.Id, ImageUrl = "/_images/service/class-1-pic-1.png", Title = "遠眺美景", Description = "白天的星月大地就似清景在新春，綠柳纔黃半未勻 黃昏時刻的星月大地彩霞滿天，最是浪漫有情天 夜晚的星月大地，月夕花朝，加上火炎山下璀璨爛漫的熠熠燈火，可謂人間仙境。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = camp.Id, ImageUrl = "/_images/service/class-1-pic-2.png", Title = "星月大地", Description = "白天的星月大地就似清景在新春，綠柳纔黃半未勻 黃昏時刻的星月大地彩霞滿天，最是浪漫有情天 夜晚的星月大地，月夕花朝，加上火炎山下璀璨爛漫的熠熠燈火，可謂人間仙境。", SortOrder = 2 });
            slides.Add(new PageSlide { PageId = camp.Id, ImageUrl = "/_images/service/class-1-pic-3.png", Title = "親子野餐", Description = "白天的星月大地就似清景在新春，綠柳纔黃半未勻 黃昏時刻的星月大地彩霞滿天，最是浪漫有情天 夜晚的星月大地，月夕花朝，加上火炎山下璀璨爛漫的熠熠燈火，可謂人間仙境。", SortOrder = 3 });
        }

        if (bbq != null)
        {
            slides.Add(new PageSlide { PageId = bbq.Id, ImageUrl = "/_images/service/class-2-pic-1.png", Title = "趣味烤肉", Description = "星月大地提供大台中微涼夜烤的新選擇", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = bbq.Id, ImageUrl = "/_images/service/class-2-pic-2.png", Title = "觀星烤肉", Description = "星月大地提供大台中微涼夜烤的新選擇", SortOrder = 2 });
            slides.Add(new PageSlide { PageId = bbq.Id, ImageUrl = "/_images/service/class-2-pic-3.png", Title = "賞月烤肉", Description = "星月大地提供大台中微涼夜烤的新選擇", SortOrder = 3 });
            slides.Add(new PageSlide { PageId = bbq.Id, ImageUrl = "/_images/service/class-2-pic-4.png", Title = "大地烤肉", Description = "星月大地提供大台中微涼夜烤的新選擇", SortOrder = 4 });
        }

        if (hotspring != null)
        {
            slides.Add(new PageSlide { PageId = hotspring.Id, ImageUrl = "/_images/service/class-3-pic-1.png", Title = "露天冷熱湯池", Description = "讓所有遠道而來的朋友們，都能在濃厚南洋異國風情的造景中，享受園區芬多精之餘，感受滋潤肌膚的美人湯品。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = hotspring.Id, ImageUrl = "/_images/service/class-3-pic-2.png", Title = "多功能SPA池", Description = "於平常忙碌擾嚷的生活中，好好放鬆、讓壓力隨汗水蒸發、一掃而空。", SortOrder = 2 });
            slides.Add(new PageSlide { PageId = hotspring.Id, ImageUrl = "/_images/service/class-3-pic-3.png", Title = "檜木烤箱", Description = "並與同行湯友，一起仰望點點繁星的浪漫夜空。", SortOrder = 3 });
            slides.Add(new PageSlide { PageId = hotspring.Id, ImageUrl = "/_images/service/class-3-pic-4.png", Title = "蒸氣室", Description = "在濃厚南洋異國風情的造景中，享受園區芬多精。", SortOrder = 4 });
            slides.Add(new PageSlide { PageId = hotspring.Id, ImageUrl = "/_images/service/class-3-pic-5.png", Title = "五星級衛浴", Description = "感受滋潤肌膚的美人湯品，好好放鬆身心。", SortOrder = 5 });
        }

        if (stay != null)
        {
            slides.Add(new PageSlide { PageId = stay.Id, ImageUrl = "/_images/service/class-3-pic-1.png", Title = "湯屋設施", Description = "每間房間設立湯屋，讓所有入住的朋友們都能放鬆身心。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = stay.Id, ImageUrl = "/_images/service/class-3-pic-2.png", Title = "北歐風格", Description = "全館以簡潔卻不失其特色的北歐風打造。", SortOrder = 2 });
        }

        if (banquet != null)
        {
            slides.Add(new PageSlide { PageId = banquet.Id, ImageUrl = "/_images/dining/class-1-pic-1.png", Title = "星級主廚", Description = "由行政總主廚領軍的廚師團隊，將在地當季食材創意入菜。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = banquet.Id, ImageUrl = "/_images/dining/class-1-pic-2.png", Title = "特色料理", Description = "有一種料理，不是論斤論兩的調味比例，而是「用愛家人的心」悉心烹調。", SortOrder = 2 });
            slides.Add(new PageSlide { PageId = banquet.Id, ImageUrl = "/_images/dining/class-1-pic-3.png", Title = "桌席宴會", Description = "景觀宴會館採整面落地窗設計，讓所有用餐的賓客都能在佳餚相伴之際，有美景相隨。", SortOrder = 3 });
        }

        if (cafe != null)
        {
            slides.Add(new PageSlide { PageId = cafe.Id, ImageUrl = "/_images/dining/class-2-pic-1.png", Title = "精選套餐", Description = "提供主廚套餐（定食、火鍋、義大利麵）及招牌蜜糖吐司、下午茶、炸物、風味飲品等多元餐飲服務。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = cafe.Id, ImageUrl = "/_images/dining/class-2-pic-2.png", Title = "浪漫下午茶", Description = "享譽全台的特製豪華蜜糖吐司席宴開放所有朋友於一周前預約", SortOrder = 2 });
            slides.Add(new PageSlide { PageId = cafe.Id, ImageUrl = "/_images/dining/class-2-pic-3.png", Title = "室內空間", Description = "可瞭望遠方山海空一線美景、享受居高臨下的廣闊景色", SortOrder = 3 });
            slides.Add(new PageSlide { PageId = cafe.Id, ImageUrl = "/_images/dining/class-2-pic-4.png", Title = "戶外環境", Description = "270度無死角觀景台，可瞭望遠方山海空一線美景、享受居高臨下的廣闊景色", SortOrder = 4 });
        }

        if (proposal != null)
        {
            slides.Add(new PageSlide { PageId = proposal.Id, ImageUrl = "/_images/wedding/class-1-pic-1.png", Title = "永遠不散的戀愛", Description = "在星月大地園區制高點上、以天地為憑證，與彼此開始一場永遠不散的戀愛。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = proposal.Id, ImageUrl = "/_images/wedding/class-1-pic-2.png", Title = "浪漫求婚", Description = "在熠熠美景相伴、精心策畫流程相隨的浪漫氛圍下求婚。", SortOrder = 2 });
        }

        if (ceremony != null)
        {
            slides.Add(new PageSlide { PageId = ceremony.Id, ImageUrl = "/_images/wedding/class-2-pic-1.png", Title = "中式文定", Description = "星月大地提供中式文定場地及布置。", SortOrder = 1 });
            slides.Add(new PageSlide { PageId = ceremony.Id, ImageUrl = "/_images/wedding/class-2-pic-2.png", Title = "歐式證婚場地", Description = "提供歐式證婚場地及布置。", SortOrder = 2 });
            slides.Add(new PageSlide { PageId = ceremony.Id, ImageUrl = "/_images/wedding/class-2-pic-3.png", Title = "歐式婚佈", Description = "客製最浪漫不俗的完美婚禮。", SortOrder = 3 });
        }

        ctx.Set<PageSlide>().AddRange(slides);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedPageTabsIfEmptyAsync(AppDbContext ctx)
    {
        if (await ctx.Set<PageTab>().AnyAsync()) return;

        var pages = await ctx.Pages.Include(p => p.Category).ToListAsync();
        Page? GetPage(string catSlug, string pageSlug) => pages.FirstOrDefault(p => p.Category.Slug == catSlug && p.Slug == pageSlug);

        var camp = GetPage("service", "camp");
        var bbq = GetPage("service", "bbq");
        var hotspring = GetPage("service", "hotspring");
        var stay = GetPage("service", "stay");
        var banquet = GetPage("dining", "banquet");
        var cafe = GetPage("dining", "cafe");
        var proposal = GetPage("wedding", "proposal");
        var ceremony = GetPage("wedding", "ceremony");

        var tabs = new List<PageTab>();

        if (camp != null)
        {
            tabs.Add(new PageTab
            {
                PageId = camp.Id, Title = "營區資訊", SortOrder = 1,
                Content = @"<p>營區配置圖</p>
<p><img src=""/_images/service/2421.png"" alt=""營區配置圖"" /></p>
<table class=""w-full border-collapse border border-gray-300"">
<tr><th class=""border border-gray-300 p-2 bg-gray-100"">海拔</th><td class=""border border-gray-300 p-2"">300m以下</td><th class=""border border-gray-300 p-2 bg-gray-100"">無線通訊</th><td class=""border border-gray-300 p-2"">中華電信有訊號 遠傳有訊號 台哥大有訊號 台灣之星有訊號</td></tr>
</table>"
            });
            tabs.Add(new PageTab { PageId = camp.Id, Title = "營地須知", SortOrder = 2, Content = "<p>營地須知內容（請透過後台管理更新）</p>" });
            tabs.Add(new PageTab { PageId = camp.Id, Title = "營區價目", SortOrder = 3, Content = "<p>營區價目內容（請透過後台管理更新）</p>" });
        }

        if (bbq != null)
        {
            tabs.Add(new PageTab
            {
                PageId = bbq.Id, Title = "烤肉資訊", SortOrder = 1,
                Content = @"<p>星月大地占廣闊、景色優美，適合闔家三五好友及公司團體，一同出遊露營(迎新)、烤肉及野餐！</p>
<p>欲洽詢露營烤肉及野餐等相關業務及詳細內容說明，歡迎撥打服務專線：04-26831671#2 中餐廳。</p>
<p><img src=""/_images/service/menu.jpg"" alt=""烤肉菜單"" /></p>"
            });
        }

        if (hotspring != null)
        {
            tabs.Add(new PageTab
            {
                PageId = hotspring.Id, Title = "相關資訊", SortOrder = 1,
                Content = "<p>星月大地風呂服務，提供露天冷熱湯池、多功能SPA池、檜木烤箱、蒸氣室、五星級衛浴等設備，讓所有遠道而來的朋友們，都能在濃厚南洋異國風情的造景中，享受園區芬多精之餘，感受滋潤肌膚的美人湯品，於平常忙碌擾嚷的生活中，好好放鬆、讓壓力隨汗水蒸發、一掃而空，並與同行湯友，一起仰望點點繁星的浪漫夜空。</p>"
            });
        }

        if (stay != null)
        {
            tabs.Add(new PageTab
            {
                PageId = stay.Id, Title = "相關資訊", SortOrder = 1,
                Content = "<p>位於河階面制高點上的星月文旅，全館以簡潔卻不失其特色的北歐風打造，更於每間房間設立湯屋，讓所有入住的朋友們，都能在旅途中放慢腳步、放鬆身心、享受在星月大地的「美」分「美」秒。同時結合外埔區、后里區的在地文化特色，客製在地深度文化旅程，是大臺中住宿的最佳選擇。</p>"
            });
        }

        if (banquet != null)
        {
            tabs.Add(new PageTab
            {
                PageId = banquet.Id, Title = "餐飲說明", SortOrder = 1,
                Content = @"<p>有一種料理，不是論斤論兩的調味比例，而是「用愛家人的心」悉心烹調。</p>
<p>星月大地景觀宴會館，由行政總主廚領軍的廚師團隊，將在地當季食材創意入菜，讓所有如家人般的顧客，能夠吃到最在地的口味，並以「愛家人的心做料理」為精神宗旨，把最好的留給您。</p>
<p>另外，景觀宴會館採整面落地窗設計，讓所有用餐的賓客都能在佳餚相伴之際，有美景相隨，更提供貴賓包廂服務，是舉辦婚禮席宴、尾牙春酒、工商宴席、多元餐聚的最佳場所。</p>
<h4>營業時間</h4>
<p>暑假開放，晚餐：</p><p>A 時段 17：30 ~ 19：00</p><p>B 時段 19：30 ~ 21：00</p>
<h4>餐廳說明</h4>
<p>全館採大片落地窗設計，讓到訪的朋友們，嘴上享用美食的同時，眼睛也能大飽美景之福。除10人桌菜外，宴會館針對不同客群也推出合菜、名廚套餐等多元餐飲服務，更結合在地與當季料理，讓大家能吃到最新鮮最有特色之佳餚。</p>
<p>同時並提供貴賓包廂供商務或大家庭餐聚，及客製菜單等服務，更是舉辦婚禮席宴的不二選擇。</p>
<h4>餐費資訊</h4>
<p>2 小時 • 桌菜5,000元/桌起(另有合菜、套餐)</p>
<p>＊景觀宴會館均消約500元/人起</p>
<p>景觀宴會館訂位專線：04-26831671#2</p>"
            });
            tabs.Add(new PageTab
            {
                PageId = banquet.Id, Title = "菜單價目", SortOrder = 2,
                Content = @"<h3>星月大地 宴會館桌菜菜單（若有變更，一律以現場提供為主）</h3>
<p><img src=""/_images/dining/menu2.jpg"" alt=""宴會館桌菜菜單"" /></p>
<h3>星月大地 宴會館合菜菜單（若有變更，一律以現場提供為主）</h3>
<p><img src=""/_images/dining/menu1.jpg"" alt=""宴會館合菜菜單"" /></p>"
            });
        }

        if (cafe != null)
        {
            tabs.Add(new PageTab
            {
                PageId = cafe.Id, Title = "餐飲說明", SortOrder = 1,
                Content = @"<p>有一件事，每天都必須重複做三次，大家卻樂此不疲，我說它是生活中最重要的小事，偶爾可以簡單平凡，卻不能隨便。</p>
<p>星月大地休閒咖啡廳擁270度無死角觀景台，可瞭望遠方山海空一線美景、享受居高臨下的廣闊景色，並提供主廚套餐（定食、火鍋、義大利麵）及招牌蜜糖吐司、下午茶、炸物、風味飲品等多元餐飲服務。</p>
<p>其中享譽全台的特製豪華蜜糖吐司席宴開放所有朋友於一周前預約，除此之外還有使用獨家醃料製成的秒殺美食北海道唐揚炸雞，一端上桌就能擄獲所有的人胃。</p>
<p>美味料理的祕訣無他，星月大地堅持、講究美饌的製程及口味，樂於與大家一同分享這件生活中最重要的小事。</p>"
            });
            tabs.Add(new PageTab
            {
                PageId = cafe.Id, Title = "菜單價目", SortOrder = 2,
                Content = @"<h3>星月大地 景觀咖啡廳菜單1（若有變更，一律以現場提供為主）</h3>
<p><img src=""/_images/dining/d02-menu1.jpg"" alt=""咖啡廳菜單1"" /></p>
<h3>星月大地 景觀咖啡廳菜單2（若有變更，一律以現場提供為主）</h3>
<p><img src=""/_images/dining/d02-menu2.jpg"" alt=""咖啡廳菜單2"" /></p>"
            });
        }

        if (proposal != null)
        {
            tabs.Add(new PageTab
            {
                PageId = proposal.Id, Title = "策劃說明", SortOrder = 1,
                Content = @"<p>永遠不散的戀愛｜求婚策畫</p>
<p>打破「婚姻就是愛情的墳墓」、「結婚就是戀愛的終點」世俗觀念，在星月大地園區制高點上、以天地為憑證，在熠熠美景相伴、精心策畫流程相隨的浪漫氛圍下，與彼此開始一場永遠不散的戀愛。</p>"
            });
        }

        if (ceremony != null)
        {
            tabs.Add(new PageTab
            {
                PageId = ceremony.Id, Title = "方案說明", SortOrder = 1,
                Content = @"<p>親愛的，這就是愛情｜婚禮席宴</p>
<p>從你們的愛情故事中，客製最浪漫不俗的完美婚禮；星月大地提供中式文定、歐式證婚場地及布置，同時提供中式圓桌席宴及百匯多元設席餐飲選擇，是舉辦婚禮席宴的不二選擇！</p>"
            });
        }

        ctx.Set<PageTab>().AddRange(tabs);
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedOrUpdateNewsCategoryAsync(AppDbContext ctx, string slug, string name, int sortOrder)
    {
        var cat = await ctx.NewsCategories.FirstOrDefaultAsync(c => c.Slug == slug);
        if (cat == null)
            ctx.NewsCategories.Add(new NewsCategory { Slug = slug, Name = name, SortOrder = sortOrder });
    }

    private static async Task SeedNewsIfEmptyAsync(AppDbContext ctx)
    {
        if (await ctx.News.AnyAsync()) return;

        var eventsCat = await ctx.NewsCategories.FirstOrDefaultAsync(c => c.Slug == "events");
        if (eventsCat == null) return;

        ctx.News.AddRange(
            new News
            {
                CategoryId = eventsCat.Id,
                Title = "星月大地粽藝大集合 包粽pk大賽！",
                Summary = "星月大地端午節特別企劃－「粽」藝大集合！包粽PK大賽！第一名現場就把3000元現金帶回家",
                Content = "<p>星月大地端午節特別企劃－「粽」藝大集合！包粽PK大賽！</p><p>第一名現場就把3000元現金帶回家！</p>",
                CoverImage = "/_images/news/news-pic-1.png",
                PublishDate = new DateTime(2025, 5, 28, 0, 0, 0, DateTimeKind.Utc),
                IsPublished = true, IsPinned = true
            },
            new News
            {
                CategoryId = eventsCat.Id,
                Title = "星樂大地跨年專案－樂yeah!越美麗",
                Summary = "星月大地跨年活動精彩不間斷，歡迎大家一同來迎接新的一年！",
                Content = "<p>星樂大地跨年專案－樂yeah!越美麗</p><p>跨年活動精彩不間斷！</p>",
                CoverImage = "/_images/news/news-pic-2.png",
                PublishDate = new DateTime(2025, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                IsPublished = true
            },
            new News
            {
                CategoryId = eventsCat.Id,
                Title = "星月大地春季活動 親子同樂會",
                Summary = "星月大地春季親子同樂活動，歡迎全家大小一起來體驗大自然的美好！",
                Content = "<p>星月大地春季親子同樂活動</p><p>歡迎全家大小一起來體驗大自然的美好！</p>",
                CoverImage = "/_images/news/news-pic-3.png",
                PublishDate = new DateTime(2025, 5, 18, 0, 0, 0, DateTimeKind.Utc),
                IsPublished = true
            }
        );
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedAlbumsIfEmptyAsync(AppDbContext ctx)
    {
        // 相簿分類
        if (!await ctx.AlbumCategories.AnyAsync())
        {
            ctx.AlbumCategories.AddRange(
                new AlbumCategory { Name = "大地活動", Slug = "events", SortOrder = 1 },
                new AlbumCategory { Name = "大地風景", Slug = "scenery", SortOrder = 2 }
            );
            await ctx.SaveChangesAsync();
        }

        if (await ctx.Albums.AnyAsync()) return;

        var eventAlbumCat = await ctx.AlbumCategories.FirstOrDefaultAsync(c => c.Slug == "events");
        if (eventAlbumCat == null) return;

        var album1 = new Album { CategoryId = eventAlbumCat.Id, Title = "大地同樂繪", CoverImage = "/_images/album/album-pic-1.png", Description = "歡慶兒童節－大地童樂繪\n讓愛遠翔竹蜻蜓彩繪DIY完美落幕", IsPublished = true, SortOrder = 1 };
        var album2 = new Album { CategoryId = eventAlbumCat.Id, Title = "戶外證婚", CoverImage = "/_images/album/album-pic-2.png", Description = "以天為憑 以地為證\n全台唯一在壯麗火炎山下的浪漫戶外證婚", IsPublished = true, SortOrder = 2 };
        var album3 = new Album { CategoryId = eventAlbumCat.Id, Title = "紅酒馬拉松", CoverImage = "/_images/album/album-pic-3.png", Description = "一年一度的紅酒馬拉松變裝嘉年華", IsPublished = true, SortOrder = 3 };
        var album4 = new Album { CategoryId = eventAlbumCat.Id, Title = "星月車聚", CoverImage = "/_images/album/album-pic-4.png", Description = "Hyundai i10第一屆年度大會師&偉士牌車聚", IsPublished = true, SortOrder = 4 };

        ctx.Albums.AddRange(album1, album2, album3, album4);
        await ctx.SaveChangesAsync();

        ctx.Set<AlbumPhoto>().AddRange(
            new AlbumPhoto { AlbumId = album1.Id, ImageUrl = "/_images/album/album-pic-1.png", Caption = "大地同樂繪", SortOrder = 1 },
            new AlbumPhoto { AlbumId = album2.Id, ImageUrl = "/_images/album/album-pic-2.png", Caption = "戶外證婚", SortOrder = 1 },
            new AlbumPhoto { AlbumId = album3.Id, ImageUrl = "/_images/album/album-pic-3.png", Caption = "紅酒馬拉松", SortOrder = 1 },
            new AlbumPhoto { AlbumId = album4.Id, ImageUrl = "/_images/album/album-pic-4.png", Caption = "星月車聚", SortOrder = 1 }
        );
        await ctx.SaveChangesAsync();
    }

    private static async Task SeedOrUpdateSettingAsync(AppDbContext ctx, string key, string value, string description)
    {
        var setting = await ctx.SiteSettings.FirstOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            ctx.SiteSettings.Add(new SiteSetting { Key = key, Value = value, Description = description });
        }
        else if (string.IsNullOrEmpty(setting.Value) && !string.IsNullOrEmpty(value))
        {
            setting.Value = value;
        }
    }

    private static async Task SeedOrUpdateTrafficAsync(AppDbContext ctx)
    {
        var busInfo = await ctx.TrafficInfos.FirstOrDefaultAsync(t => t.TabName == "市區公車");
        if (busInfo != null)
        {
            busInfo.Content = @"<ul>
<li><strong>豐原客運214號：</strong>后里馬場或大甲火車站發車→六分月眉北路口站牌下車→步行約5分鐘抵達</li>
<li><strong>豐原客運215號：</strong>豐原或大甲體育場發車→麗寶樂園站下車→步行約20分鐘抵達</li>
<li><strong>統聯客運155號：</strong>高鐵台中站或麗寶樂園發車→麗寶樂園站下車→步行約20分鐘抵達</li>
<li><strong>統聯客運155副線：</strong>高鐵台中站或麗寶樂園發車→月眉北路口站牌下車→步行約15分鐘抵達</li>
</ul>";
        }
        else if (!await ctx.TrafficInfos.AnyAsync())
        {
            ctx.TrafficInfos.Add(new TrafficInfo
            {
                TabName = "市區公車", SortOrder = 1,
                Content = @"<ul>
<li><strong>豐原客運214號：</strong>后里馬場或大甲火車站發車→六分月眉北路口站牌下車→步行約5分鐘抵達</li>
<li><strong>豐原客運215號：</strong>豐原或大甲體育場發車→麗寶樂園站下車→步行約20分鐘抵達</li>
<li><strong>統聯客運155號：</strong>高鐵台中站或麗寶樂園發車→麗寶樂園站下車→步行約20分鐘抵達</li>
<li><strong>統聯客運155副線：</strong>高鐵台中站或麗寶樂園發車→月眉北路口站牌下車→步行約15分鐘抵達</li>
</ul>"
            });
        }

        var driveInfo = await ctx.TrafficInfos.FirstOrDefaultAsync(t => t.TabName == "自行開車");
        if (driveInfo != null)
        {
            driveInfo.Content = @"<ul>
<li><strong>走國道1號：</strong>國1下→往外埔（大摩天輪）方向→約3分鐘車程即可抵達</li>
<li><strong>走國道3號：</strong>國3下→往后里（大摩天輪）方向→約5至10分鐘車程可抵達</li>
</ul>";
        }
        else if (await ctx.TrafficInfos.CountAsync() < 2)
        {
            ctx.TrafficInfos.Add(new TrafficInfo
            {
                TabName = "自行開車", SortOrder = 2,
                Content = @"<ul>
<li><strong>走國道1號：</strong>國1下→往外埔（大摩天輪）方向→約3分鐘車程即可抵達</li>
<li><strong>走國道3號：</strong>國3下→往后里（大摩天輪）方向→約5至10分鐘車程可抵達</li>
</ul>"
            });
        }

        // 更新電話、地址等設定（即使已有值也強制更新為正確的）
        var phoneSetting = await ctx.SiteSettings.FirstOrDefaultAsync(s => s.Key == "phone");
        if (phoneSetting != null && phoneSetting.Value != "04-26831671")
            phoneSetting.Value = "04-26831671";

        var addressSetting = await ctx.SiteSettings.FirstOrDefaultAsync(s => s.Key == "address");
        if (addressSetting != null && !addressSetting.Value.Contains("486"))
            addressSetting.Value = "台中市后里區月眉北路486號(大摩天輪正後方)";

        var mapsSetting = await ctx.SiteSettings.FirstOrDefaultAsync(s => s.Key == "google_maps_embed");
        if (mapsSetting != null && string.IsNullOrEmpty(mapsSetting.Value))
            mapsSetting.Value = "https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d14541.597505171469!2d120.701191!3d24.332577!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0xf22f690b198e3e44!2z5pif5pyI5aSn5Zyw5LyR6ZaS5LqL5qWt5pyJ6ZmQ5YWs5Y-4!5e0!3m2!1sen!2shk!4v1465960095168";

        await ctx.SaveChangesAsync();
    }

    private static async Task FixImageUrlPrefixesAsync(AppDbContext ctx)
    {
        // 修正所有不含 / 前綴的圖片路徑
        var news = await ctx.News.Where(n => n.CoverImage != null && !n.CoverImage.StartsWith("/")).ToListAsync();
        foreach (var n in news) n.CoverImage = $"/_images/{n.CoverImage}";

        var albums = await ctx.Albums.Where(a => a.CoverImage != null && !a.CoverImage.StartsWith("/")).ToListAsync();
        foreach (var a in albums) a.CoverImage = $"/_images/{a.CoverImage}";

        var photos = await ctx.Set<AlbumPhoto>().Where(p => !p.ImageUrl.StartsWith("/")).ToListAsync();
        foreach (var p in photos) p.ImageUrl = $"/_images/{p.ImageUrl}";

        var homepageSlides = await ctx.HomepageSlides.Where(h => !h.ImageUrl.StartsWith("/")).ToListAsync();
        foreach (var h in homepageSlides) h.ImageUrl = $"/_images/{h.ImageUrl}";

        var categories = await ctx.PageCategories.Where(c => c.BannerImage != null && !c.BannerImage.StartsWith("/")).ToListAsync();
        foreach (var c in categories) c.BannerImage = $"/_images/{c.BannerImage}";

        if (news.Count + albums.Count + photos.Count + homepageSlides.Count + categories.Count > 0)
            await ctx.SaveChangesAsync();
    }

    private static async Task SeedHomepageSlidesAsync(AppDbContext ctx)
    {
        var slides = new[]
        {
            new { ImageUrl = "/_images/index/banner-01.png", AltText = "星月大地景觀休閒園區", SortOrder = 1 },
            new { ImageUrl = "/_images/index/banner-02.png", AltText = "大地景觀", SortOrder = 2 },
            new { ImageUrl = "/_images/index/banner-03.png", AltText = "星月餐飲", SortOrder = 3 },
            new { ImageUrl = "/_images/index/banner-04.png", AltText = "星月訊息", SortOrder = 4 },
        };

        foreach (var s in slides)
        {
            var existing = await ctx.HomepageSlides.FirstOrDefaultAsync(h => h.ImageUrl == s.ImageUrl);
            if (existing == null)
            {
                ctx.HomepageSlides.Add(new HomepageSlide
                {
                    ImageUrl = s.ImageUrl,
                    AltText = s.AltText,
                    SortOrder = s.SortOrder,
                    IsActive = true
                });
            }
        }
        await ctx.SaveChangesAsync();
    }
}
