using FluentValidation;
using StarMoonLand.Core.Entities;

namespace StarMoonLand.Api.Validators;

public class PageCategoryValidator : AbstractValidator<PageCategory>
{
    public PageCategoryValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("請輸入 Slug")
            .MaximumLength(100).WithMessage("Slug 最多 100 個字元");

        RuleFor(x => x.TitleZh)
            .NotEmpty().WithMessage("請輸入中文標題")
            .MaximumLength(200).WithMessage("中文標題最多 200 個字元");

        RuleFor(x => x.TitleEn)
            .NotEmpty().WithMessage("請輸入英文標題")
            .MaximumLength(200).WithMessage("英文標題最多 200 個字元");
    }
}

public class PageValidator : AbstractValidator<Page>
{
    public PageValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("請輸入 Slug")
            .MaximumLength(100).WithMessage("Slug 最多 100 個字元");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("請輸入標題")
            .MaximumLength(200).WithMessage("標題最多 200 個字元");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("請選擇分類");
    }
}

public class NewsValidator : AbstractValidator<News>
{
    public NewsValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("請輸入標題")
            .MaximumLength(500).WithMessage("標題最多 500 個字元");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("請選擇分類");
    }
}

public class AlbumValidator : AbstractValidator<Album>
{
    public AlbumValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("請輸入標題")
            .MaximumLength(200).WithMessage("標題最多 200 個字元");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("請選擇分類");
    }
}

public class HomepageSlideValidator : AbstractValidator<HomepageSlide>
{
    public HomepageSlideValidator()
    {
        RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("圖片路徑不可為空");
    }
}

public class TrafficInfoValidator : AbstractValidator<TrafficInfo>
{
    public TrafficInfoValidator()
    {
        RuleFor(x => x.TabName).NotEmpty().WithMessage("Tab 名稱不可為空");
    }
}
