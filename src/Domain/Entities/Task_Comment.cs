namespace Domain.Entities
{
    public class Task_Comment
    {
        public int TaskId { get; set; }
        public UserTask UserTask { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
