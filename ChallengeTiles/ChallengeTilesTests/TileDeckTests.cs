using ChallengeTiles.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeTilesTests
{
    internal class TileDeckTests
    {
        [Test]
        public void CreateTileDeck_CreatesCorrectNumberOfTiles()
        {
            //Arrange
            int playedColors = 2; // Assume two colors are selected
            int expectedCount = (Constants.TileMax - Constants.TileMin + 1) * playedColors;
            TileDeck deck = new TileDeck(playedColors);

            //Act
            deck.CreateTileDeck();

            //Assert
            Assert.That(deck.GetTileCount(), Is.EqualTo(expectedCount));
        }

        [Test]
        public void ShuffleTiles_ChangesTileOrder()
        {
            //Arrange
            TileDeck deck = new TileDeck(2);
            deck.CreateTileDeck();
            List<Tile> originalOrder = new List<Tile>(deck.GetTiles());

            //Act
            deck.ShuffleTiles();
            List<Tile> shuffledOrder = deck.GetTiles();

            //Assert
            Assert.That(shuffledOrder, Is.Not.EqualTo(originalOrder));
        }

        [Test]
        public void GetTile_ReturnsCorrectTile()
        {
            //Arrange
            TileDeck deck = new TileDeck(1);
            deck.CreateTileDeck();

            //Act
            Tile tile = deck.GetTile(0);

            //Assert
            Assert.NotNull(tile);
        }

        [Test]
        public void RemoveTile_RemovesTileAndReturnsIt()
        {
            //Arrange
            TileDeck deck = new TileDeck(1);
            deck.CreateTileDeck();
            int initialCount = deck.GetTileCount();

            //Act
            Tile removedTile = deck.RemoveTile(0);

            //Assert
            Assert.That(deck.GetTileCount(), Is.EqualTo(initialCount - 1));
            Assert.NotNull(removedTile);
        }

        [Test]
        public void ResetTileDeck_ResetsAndShuffles()
        {
            //Arrange
            TileDeck deck = new TileDeck(1);
            deck.CreateTileDeck();
            deck.ShuffleTiles();
            List<Tile> shuffledDeck = new List<Tile>(deck.GetTiles());

            //Act
            deck.ResetTileDeck();
            List<Tile> resetDeck = deck.GetTiles();

            //Assert
            Assert.That(resetDeck, Is.Not.EqualTo(shuffledDeck));
            Assert.That(deck.GetTileCount(), Is.EqualTo((Constants.TileMax - Constants.TileMin + 1)));
        }
    }
}
