﻿using System.Collections.Generic;

namespace Domain.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public string Path { get; set; }

        public List<Avatar> Avatars { get; set; } 
    }

    public enum Type
    {
        Image = 0,
        Sound,
        Video,
        Document,
        File
    }
}
