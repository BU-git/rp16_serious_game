using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public ApplicationUser Author { get; set; }
        public string Text { get; set; }
        public DateTime EditDate { get; set; }
        public Media Image { get; set; }

        public Comment Parent { get; set; }
        public Task_Comment Task_Comment { get; set; }
        public List<Comment> Replies { get; set; }
    }
}
