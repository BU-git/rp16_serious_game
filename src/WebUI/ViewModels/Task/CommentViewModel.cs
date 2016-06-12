using System.Collections.Generic;

namespace WebUI.ViewModels.Task
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }

        public List<CommentViewModel> Children { get; set; }
    }
}
