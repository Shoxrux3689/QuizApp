namespace IdentitySignUp.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string QuestionText { get; set; }
        public virtual List<Choice> Choices { get; set; }

        public Question()
        {
            Choices = new List<Choice>();
        }
    }
}
