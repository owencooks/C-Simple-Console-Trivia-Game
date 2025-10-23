namespace TriviaGame.Shared
{
    public static class QuestionBank
    {
        // List of all the questions that can be asked
        public static List<TriviaQuestion> GetQuestions()
        {
            var questions = new List<TriviaQuestion>();

            questions.Add(new TriviaQuestion(
                "What is the capital of France?",
                new List<string> { "London", "Paris", "Berlin", "Madrid" },
                1));

            questions.Add(new TriviaQuestion(
                "What is 2 + 2?",
                new List<string> { "3", "4", "5", "6" },
                1));

            questions.Add(new TriviaQuestion(
                "Which planet is known as the Red Planet?",
                new List<string> { "Venus", "Jupiter", "Mars", "Saturn" },
                2));

            questions.Add(new TriviaQuestion(
                "What is the largest ocean on Earth?",
                new List<string> { "Atlantic", "Indian", "Arctic", "Pacific" },
                3));

            questions.Add(new TriviaQuestion(
                "Who wrote 'Romeo and Juliet'?",
                new List<string> { "Charles Dickens", "William Shakespeare", "Mark Twain", "Jane Austen" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the smallest prime number?",
                new List<string> { "0", "1", "2", "3" },
                2));

            questions.Add(new TriviaQuestion(
                "How many continents are there?",
                new List<string> { "5", "6", "7", "8" },
                2));

            questions.Add(new TriviaQuestion(
                "What is the chemical symbol for gold?",
                new List<string> { "Go", "Gd", "Au", "Ag" },
                2));

            questions.Add(new TriviaQuestion(
                "What year did World War II end?",
                new List<string> { "1943", "1944", "1945", "1946" },
                2));

            questions.Add(new TriviaQuestion(
                "How many sides does a hexagon have?",
                new List<string> { "4", "5", "6", "7" },
                2));

            questions.Add(new TriviaQuestion(
                "What is the largest mammal in the world?",
                new List<string> { "African Elephant", "Blue Whale", "Giraffe", "Polar Bear" },
                1));

            questions.Add(new TriviaQuestion(
                "Who painted the Mona Lisa?",
                new List<string> { "Vincent van Gogh", "Leonardo da Vinci", "Pablo Picasso", "Michelangelo" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the speed of light?",
                new List<string> { "299,792 km/s", "150,000 km/s", "500,000 km/s", "100,000 km/s" },
                0));

            questions.Add(new TriviaQuestion(
                "How many bones are in the human body?",
                new List<string> { "186", "206", "256", "306" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the smallest country in the world?",
                new List<string> { "Monaco", "Vatican City", "San Marino", "Liechtenstein" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the hardest natural substance on Earth?",
                new List<string> { "Gold", "Iron", "Diamond", "Platinum" },
                2));

            questions.Add(new TriviaQuestion(
                "Which gas do plants absorb from the atmosphere?",
                new List<string> { "Oxygen", "Carbon Dioxide", "Nitrogen", "Hydrogen" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the capital of Japan?",
                new List<string> { "Seoul", "Beijing", "Tokyo", "Bangkok" },
                2));

            questions.Add(new TriviaQuestion(
                "How many strings does a standard guitar have?",
                new List<string> { "4", "5", "6", "7" },
                2));

            questions.Add(new TriviaQuestion(
                "What is the largest planet in our solar system?",
                new List<string> { "Saturn", "Jupiter", "Neptune", "Uranus" },
                1));

            questions.Add(new TriviaQuestion(
                "Who wrote 'Harry Potter'?",
                new List<string> { "J.R.R. Tolkien", "J.K. Rowling", "C.S. Lewis", "George R.R. Martin" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the boiling point of water in Celsius?",
                new List<string> { "90°C", "100°C", "110°C", "120°C" },
                1));

            questions.Add(new TriviaQuestion(
                "How many days are in a leap year?",
                new List<string> { "365", "366", "367", "364" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the currency of the United Kingdom?",
                new List<string> { "Euro", "Pound Sterling", "Dollar", "Franc" },
                1));

            questions.Add(new TriviaQuestion(
                "Which element has the chemical symbol 'O'?",
                new List<string> { "Gold", "Osmium", "Oxygen", "Oganesson" },
                2));

            questions.Add(new TriviaQuestion(
                "How many players are on a soccer team?",
                new List<string> { "9", "10", "11", "12" },
                2));

            questions.Add(new TriviaQuestion(
                "What is the tallest mountain in the world?",
                new List<string> { "K2", "Mount Everest", "Kilimanjaro", "Denali" },
                1));

            questions.Add(new TriviaQuestion(
                "Which country is home to the kangaroo?",
                new List<string> { "New Zealand", "Australia", "South Africa", "Brazil" },
                1));

            questions.Add(new TriviaQuestion(
                "What year did the Titanic sink?",
                new List<string> { "1905", "1912", "1920", "1898" },
                1));

            questions.Add(new TriviaQuestion(
                "How many minutes are in a full day?",
                new List<string> { "1440", "1400", "1480", "1500" },
                0));

            questions.Add(new TriviaQuestion(
                "What is the longest river in the world?",
                new List<string> { "Amazon", "Nile", "Yangtze", "Mississippi" },
                1));

            questions.Add(new TriviaQuestion(
                "Which programming language is known for web development?",
                new List<string> { "C++", "Python", "JavaScript", "Assembly" },
                2));

            questions.Add(new TriviaQuestion(
                "How many hearts does an octopus have?",
                new List<string> { "1", "2", "3", "4" },
                2));

            questions.Add(new TriviaQuestion(
                "What is the freezing point of water in Fahrenheit?",
                new List<string> { "0°F", "32°F", "100°F", "212°F" },
                1));

            questions.Add(new TriviaQuestion(
                "Which planet is closest to the Sun?",
                new List<string> { "Venus", "Mercury", "Earth", "Mars" },
                1));

            questions.Add(new TriviaQuestion(
                "How many legs does a spider have?",
                new List<string> { "6", "8", "10", "12" },
                1));

            questions.Add(new TriviaQuestion(
                "What is the capital of Canada?",
                new List<string> { "Toronto", "Vancouver", "Ottawa", "Montreal" },
                2));

            questions.Add(new TriviaQuestion(
                "Which country invented pizza?",
                new List<string> { "Greece", "Italy", "France", "Spain" },
                1));

            questions.Add(new TriviaQuestion(
                "How many colors are in a rainbow?",
                new List<string> { "5", "6", "7", "8" },
                2));

            return questions;
        }
    }
}