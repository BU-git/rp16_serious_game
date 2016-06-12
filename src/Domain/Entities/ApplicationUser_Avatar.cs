namespace Domain.Entities
{
    public class ApplicationUser_Avatar
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int AvatarId { get; set; }
        public Avatar Avatar { get; set; }
    }
}
