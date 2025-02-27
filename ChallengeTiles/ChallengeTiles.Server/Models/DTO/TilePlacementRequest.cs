﻿namespace ChallengeTiles.Server.Models.DTO
{
    public class TilePlacementRequest
    {
        //GameId sent in URL in RESTful API's, have for added flexibility
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public Tile Tile { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
