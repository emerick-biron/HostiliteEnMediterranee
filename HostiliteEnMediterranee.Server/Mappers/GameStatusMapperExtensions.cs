using HostiliteEnMediterranee.Models.Dto;
using HostiliteEnMediterranee.Server.Entities;

namespace HostiliteEnMediterranee.Server.Mappers;

public static class GameStatusMapperExtensions
{
    public static GameStatusDto ToDto(this GameStatus gameStatus)
    {
        return gameStatus switch
        {
            GameStatus.NotStarted => GameStatusDto.NotStarted,
            GameStatus.InProgress => GameStatusDto.InProgress,
            GameStatus.Over => GameStatusDto.Over,
            _ => throw new ArgumentOutOfRangeException(nameof(gameStatus), gameStatus, null)
        };
    }

    public static Proto.GameStatusDto ToProto(this GameStatus gameStatus)
    {
        return gameStatus switch
        {
            GameStatus.NotStarted => Proto.GameStatusDto.NotStarted,
            GameStatus.InProgress => Proto.GameStatusDto.InProgress,
            GameStatus.Over => Proto.GameStatusDto.Over,
            _ => throw new ArgumentOutOfRangeException(nameof(gameStatus), gameStatus, null)
        };
    }
}