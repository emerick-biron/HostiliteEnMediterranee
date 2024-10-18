using FluentValidation;
using HostiliteEnMediterranee.Models.Requests;

namespace HostiliteEnMediterranee.Server.Validators;

public class StartGameRequestValidator : AbstractValidator<StartGameRequest>
{
    public StartGameRequestValidator()
    {
        RuleFor(request => request.AiLevel).NotNull().WithMessage("AI level is required");
    }
}