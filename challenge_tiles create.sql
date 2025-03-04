-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema challenge_tiles
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema challenge_tiles
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `challenge_tiles` DEFAULT CHARACTER SET utf8 ;
USE `challenge_tiles` ;

-- -----------------------------------------------------
-- Table `challenge_tiles`.`Game`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `challenge_tiles`.`Game` (
  `GameId` INT NOT NULL AUTO_INCREMENT,
  `NumberOfColors` INT NOT NULL,
  `NumberOfTiles` INT NOT NULL,
  `Score` INT NOT NULL,
  PRIMARY KEY (`GameId`),
  UNIQUE INDEX `GameId_UNIQUE` (`GameId` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `challenge_tiles`.`Player`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `challenge_tiles`.`Player` (
  `PlayerId` INT NOT NULL AUTO_INCREMENT,
  `Username` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(45) NOT NULL,
  `Name` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`PlayerId`),
  UNIQUE INDEX `UserId_UNIQUE` (`PlayerId` ASC) VISIBLE,
  UNIQUE INDEX `Username_UNIQUE` (`Username` ASC) VISIBLE,
  UNIQUE INDEX `Email_UNIQUE` (`Email` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `challenge_tiles`.`Hand`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `challenge_tiles`.`Hand` (
  `HandId` INT NOT NULL AUTO_INCREMENT,
  `Tiles` VARCHAR(45) NOT NULL,
  `PlayerId` INT NOT NULL,
  `GameId` INT NOT NULL,
  PRIMARY KEY (`HandId`, `PlayerId`, `GameId`),
  UNIQUE INDEX `HandId_UNIQUE` (`HandId` ASC) VISIBLE,
  UNIQUE INDEX `GameId_UNIQUE` (`GameId` ASC) VISIBLE,
  INDEX `fk_Hand_Player_idx` (`PlayerId` ASC) VISIBLE,
  INDEX `fk_Hand_Game1_idx` (`GameId` ASC) VISIBLE,
  CONSTRAINT `fk_Hand_Player`
    FOREIGN KEY (`PlayerId`)
    REFERENCES `challenge_tiles`.`Player` (`PlayerId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Hand_Game1`
    FOREIGN KEY (`GameId`)
    REFERENCES `challenge_tiles`.`Game` (`GameId`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
