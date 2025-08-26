using System;

/// <summary>
/// Summary description for Class1
/// </summary>
public static class GameMapper
{
	public GameMapper()
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
