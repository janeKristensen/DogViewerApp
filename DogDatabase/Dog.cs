


namespace DogDatabase
{
    public class Dog
    {
        private int _excersizeLevel;

        public Dog(string breed) { BreedName = breed; }

        public Dog(string breed, string subbreed) : this(breed)
        { 
            SubBreed = subbreed;
        }

        public Dog(string breed, string subbreed, string coatLength, string size, double avgAge, string temper, int excersize) : this(breed, subbreed)
        {
            
            CoatLength = coatLength;    
            Size = size;
            AverageAge = avgAge;
            Temper = temper;
            ExcersizeLevel = excersize;
        }

        public string BreedName { get; set; }
        public string SubBreed { get; set; } = string.Empty;
        public string CoatLength { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public double AverageAge { get; set; } = 0;
        public string Temper { get; set; } = string.Empty;
        public int ExcersizeLevel 
        {
            get { return _excersizeLevel; }
            set 
            {
                if (value < 1 || value > 5)
                    throw new ArgumentException("Excersize level must be a value between 1 and 5.");

                _excersizeLevel = value;
            }
        }

        public override string ToString()
        {
            return BreedName + SubBreed; 
        }
    }
}
