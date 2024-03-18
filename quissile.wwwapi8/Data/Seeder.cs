using quissile.wwwapi8.Models;
using System.Security.Cryptography.X509Certificates;

namespace quissile.wwwapi8.Data
{
    public class Seeder
    {

            private List<Question> _questions = new List<Question>();
            private List<Alternative> _alternatives = [];
           // private List<QuestionAlternative> _question_alternatives = new List<QuestionAlternative>();
           // private List<Quiz> _quizes = new List<Quiz>();
           // private List<QuizQuestion> _quizQuestions = new List<QuizQuestion>();
            public Seeder() { 
        
            _questions.Add(
                new Question { Id = 1, Text = "Hva står API for?" }
            );

            _alternatives = new List<Alternative>
            {
                new Alternative { Id = 1, Text = "Application Programming Interface", Answer = true, QuestionId = 1 },
                new Alternative { Id = 2, Text = "Application Project Interface", Answer = false, QuestionId = 1 }
            };
            /*
            _question_alternatives.Add(
                new QuestionAlternative { Id = 1 }
            );
            _quizes.Add(
                new Quiz { Id = 1, Title = "Progge - quiz"}
            );
            _quizQuestions.Add(
                new QuizQuestion { Id = 1, QuestionAlternativeId = 1, QuizId = 1 }
            );
            */
        }

        public List<Question> Questions { get { return _questions; } }
        public List<Alternative> Alternatives { get { return _alternatives; } }
       // public List<Quiz> Quizes { get { return _quizes; } }
       // public List<QuestionAlternative> QuestionAlternatives { get { return _question_alternatives; } }
       // public List<QuizQuestion> QuizQuestions { get { return _quizQuestions; } }


    }
}
