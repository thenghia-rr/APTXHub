namespace APTXHub.ViewModels.Setttings
{
    public class UpdateUserPictureVM
    {
        // Avatar
        public IFormFile ProfilePictureImage { get; set; }

        // Cover
        public IFormFile CoverImage { get; set; }
    }
}
