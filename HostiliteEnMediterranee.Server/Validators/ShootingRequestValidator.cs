using FluentValidation;
using HostiliteEnMediterranee.Models.Requests;
using HostiliteEnMediterranee.Server.Entities;

namespace HostiliteEnMediterranee.Server.Validators;

public class ShootingRequestValidator : AbstractValidator<ShootingRequest>
{
    public ShootingRequestValidator()
    {
        RuleFor(x => x.ShootCoordinates)
            .NotNull().WithMessage("ShootCoordinates is required");

        RuleFor(x => x.ShootCoordinates.Row)
            .InclusiveBetween(0, Player.GridSize - 1)
            .WithMessage($"Coordinates.Row must be between 0 and {Player.GridSize - 1}");

        RuleFor(x => x.ShootCoordinates.Column)
            .InclusiveBetween(0, Player.GridSize - 1)
            .WithMessage($"Coordinates.Column must be between 0 and {Player.GridSize - 1}");
    }
}