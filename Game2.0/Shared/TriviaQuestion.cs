namespace TriviaGame.Shared
{
    // This section is for a single question that has multiple choice answers
    public class TriviaQuestion
    {
         //Question to display to players
        public string Question { get; set; }
        // 4 Question Answers
        public List<string> Options { get; set; }
        // Finding the correct answer that the user enters
        public int CorrectAnswer { get; set; }

        //This section of code creates a new trivia question with the question,
        // the options for the question, and the correct answer for the question
        public TriviaQuestion(string question, List<string> options, int correctAnswer)
        {
            Question = question;
            Options = options;
            CorrectAnswer = correctAnswer;
        }
    }
}