using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ChallengeTiles.Server.Models;

namespace ChallengeTilesTests
{
    internal class TileTests
    {      
        [Test]
        //Test that a tile object can be created with correct values
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange: tile test data
            int expectedId = 1;
            int expectedNumber = 5;
            string expectedColor = "Red";

            // Act: create new tile object
            Tile tile = new Tile(expectedId, expectedNumber, expectedColor);

            // Assert: check if values are correct
            Assert.That(tile.Id, Is.EqualTo(expectedId));
            Assert.That(tile.Number, Is.EqualTo(expectedNumber));
            Assert.That(tile.Color, Is.EqualTo(expectedColor));
        }

        [Test]
        //check image is set correctly
        public void Constructor_ShouldGenerateCorrectTileImageUrl()
        {
            //Arrange: tile attribute values
            int number = 3;
            string color = "Blue";

            //Act: create tile object
            Tile tile = new Tile(1, number, color);

            //Assert: check if correct image is assigned to the tile object
            string expectedUrl = $"{Constants.BucketUrl}/tile_{number}_{color}";
            Assert.That(tile.TileImageUrl, Is.EqualTo(expectedUrl));
        }

        [Test]
        //test the correct ToString is returned for tile object
        public void ToString_ShouldReturnCorrectFormat()
        {
            //Arrange: Instantiate tile object
            Tile tile = new Tile(2, 7, "Green");

            //Act: ToString for tile object
            string result = tile.ToString();

            //Assert: Correct ToString
            Assert.That(result, Is.EqualTo("Green7"));
        }

        //Equals tests
        [Test]
        //true test
        public void Equals_ShouldReturnTrue_ForSameTileValues()
        {
            //Arrange: create 2 tile objects with same values
            Tile tile1 = new Tile(1, 4, "Yellow");
            Tile tile2 = new Tile(1, 4, "Yellow");

            //Act & Assert: verify they are equal
            Assert.That(tile1.Equals(tile2), Is.True);
        }

        [Test]
        //false test
        public void Equals_ShouldReturnFalse_ForDifferentTileValues()
        {
            //Arrange: create 2 tile objects with different values
            Tile tile1 = new Tile(1, 4, "Yellow");
            Tile tile2 = new Tile(2, 4, "Yellow");

            //Act & Assert: verify they are not equal
            Assert.That(tile1.Equals(tile2), Is.False);
        }

        [Test]
        //Hashcode test
        public void GetHashCode_ShouldBeSame_ForEqualObjects()
        {
            //Arrange: create 2 tile objects of same value
            Tile tile1 = new Tile(1, 4, "Yellow");
            Tile tile2 = new Tile(1, 4, "Yellow");

            //Act & Assert: check hascodes for each are the same
            Assert.That(tile1.GetHashCode(), Is.EqualTo(tile2.GetHashCode()));
        }
    }
}
