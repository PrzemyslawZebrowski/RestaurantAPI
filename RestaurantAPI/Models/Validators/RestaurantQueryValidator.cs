using System.Linq;
using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators;

public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
{
    private readonly int[] _allowedPageSizes = {5, 10, 15};

    private readonly string[] _allowedSortByColumnNames =
        {nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description)};

    public RestaurantQueryValidator()
    {
        RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).Custom((size, context) =>
        {
            if (!_allowedPageSizes.Contains(size))
                context.AddFailure("PageSize", $"PageSize must be in [{string.Join(",", _allowedPageSizes)}]");
        });
        RuleFor(r => r.SortBy).Custom((value, context) =>
        {
            if (!string.IsNullOrEmpty(value) && !_allowedSortByColumnNames.Contains(value))
                context.AddFailure($"SortBy must be empty or in [{string.Join(", ", _allowedSortByColumnNames)}]");
        });
    }
}