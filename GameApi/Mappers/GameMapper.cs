using System;
using GameApi.DTO;
using GameApi.Models;

namespace GameApi.Mappers
{
	public class GameMapper()
	{
		
		public static GameDto ToDto(Game game)
		{
			return new GameDto
			{
				GameId = game.GameId,
				Name = game.Name,
				Image = game.Image,
				Link = game.Link,
				Price = game.Price,
				Discount = game.Discount
			};
		}
		public static Game ToEntity(GameDto gameDto)
		{
			return new Game
			{
				GameId = gameDto.GameId,
				Name = gameDto.Name,
				Image = gameDto.Image,
				Link = gameDto.Link,
				Price = gameDto.Price,
				Discount = gameDto.Discount
			};
    }
}
}


