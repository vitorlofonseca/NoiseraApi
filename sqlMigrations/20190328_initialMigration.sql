-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema noiseradb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema noiseradb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `noiseradb` DEFAULT CHARACTER SET utf8 ;
USE `noiseradb` ;

-- -----------------------------------------------------
-- Table `noiseradb`.`gigs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`gigs` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `description` VARCHAR(500) NULL,
  `avatar_url` VARCHAR(100) NOT NULL,
  `spotify_playlist_id` INT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `noiseradb`.`songs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`songs` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `artist` VARCHAR(45) NOT NULL,
  `spotify_song_id` INT NULL,
  `avatar_url` VARCHAR(100) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `noiseradb`.`songs_gig`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`songs_gig` (
  `gig_id` INT NOT NULL,
  `song_id` INT NOT NULL,
  `active` TINYINT NOT NULL,
  `order` INT NOT NULL,
  PRIMARY KEY (`gig_id`, `song_id`),
  INDEX `fk_songs_playlist_tryouts_idx` (`gig_id` ASC),
  INDEX `fk_songs_playlist_songs1_idx` (`song_id` ASC),
  CONSTRAINT `fk_songs_playlist_tryouts`
    FOREIGN KEY (`gig_id`)
    REFERENCES `noiseradb`.`gigs` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_songs_playlist_songs1`
    FOREIGN KEY (`song_id`)
    REFERENCES `noiseradb`.`songs` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `noiseradb`.`user`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`user` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `email` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `noiseradb`.`role_gig`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`role_gig` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `role` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `noiseradb`.`user_gig`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`user_gig` (
  `gig_id` INT NOT NULL,
  `user_id` INT NOT NULL,
  `role_gig_id` INT NOT NULL,
  PRIMARY KEY (`gig_id`, `user_id`),
  INDEX `fk_user_tryout_user1_idx` (`user_id` ASC),
  INDEX `fk_user_tryout_role_tryout1_idx` (`role_gig_id` ASC),
  CONSTRAINT `fk_user_tryout_tryouts1`
    FOREIGN KEY (`gig_id`)
    REFERENCES `noiseradb`.`gigs` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_tryout_user1`
    FOREIGN KEY (`user_id`)
    REFERENCES `noiseradb`.`user` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_tryout_role_tryout1`
    FOREIGN KEY (`role_gig_id`)
    REFERENCES `noiseradb`.`role_gig` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `noiseradb`.`dates_gigs`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `noiseradb`.`dates_gigs` (
  `gigs_int` INT NOT NULL,
  `date` DATETIME NOT NULL,
  PRIMARY KEY (`gigs_int`),
  CONSTRAINT `fk_dates_tryouts_tryouts1`
    FOREIGN KEY (`gigs_int`)
    REFERENCES `noiseradb`.`gigs` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
