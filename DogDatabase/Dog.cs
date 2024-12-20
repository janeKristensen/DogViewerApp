﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace DogDatabase
{
    public class Dog()
    {
        private byte _excersizeLevel;

        public Dog(string BreedName):this() 
        { 
            this.BreedName = BreedName; 
        }

        public Dog(string BreedName, string SubBreed) : this(BreedName)
        { 
            this.SubBreed = SubBreed;
        }

        public Dog(int id, string BreedName, string SubBreed, string coatLength, string size, 
            double avgAge,  string temper, byte excersize)  : this(BreedName, SubBreed)
        { 
            Id = id;
            CoatLength = coatLength;    
            Size = size;
            AverageAge = avgAge;
            Temper = temper;
            ExcersizeLevel = excersize;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string BreedName { get; init; }
        public string SubBreed { get; init; } = string.Empty;
        public string CoatLength { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public double AverageAge { get; set; } = 0;
        public string Temper { get; set; } = string.Empty;
        public int Score {  get; set; } = 0;
        public int Ratings { get; set; } = 0;
        public Decimal Stars { get; set; } = 0;

        public byte ExcersizeLevel
        {
            get { return _excersizeLevel; }
            set
            {
                if (value < 0 || value > 5)
                    throw new ArgumentException("Excersize level must be a value between 1 and 5.");

                _excersizeLevel = value;
            }
        }
        
        public override string ToString()
        {
            return $"{ BreedName} { SubBreed}"; 
        }

        public void AddRating(int value)
        {
            Ratings++;
            Score += value;
        }

        public Decimal GetRating()
        {
            Stars = (Decimal)Score / (Decimal)Ratings;
            return Stars;
        }
    }
}
