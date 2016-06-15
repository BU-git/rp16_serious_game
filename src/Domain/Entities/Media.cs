namespace Domain.Entities
{
    public class Media
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public string MainPath { get; set; }
        public string AdditionalPath { get; set; }
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
