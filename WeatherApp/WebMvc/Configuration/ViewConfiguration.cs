namespace WebMvc.Configuration
{
    /// <summary>
    /// Configuration injected into views. No interface because views won't use those.
    /// </summary>
    public class ViewConfiguration
    {
        public const string Section = "ViewConfiguration";
        public string ApiDocumentationUrl { get; set; }
    }
}