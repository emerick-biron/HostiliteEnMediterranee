using HostiliteEnMediterranee.Models.Dto;

namespace HostiliteEnMediterranee.Models.Responses;

public record UndoLastPlayerTurnResponse(List<ShotDto> UndoneShots);