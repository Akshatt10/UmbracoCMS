using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Microsoft.Extensions.Logging;

namespace UmbracoProject.Handlers
{
    public class ModuleEventHandler : INotificationHandler<ContentSavingNotification>
    {
        private readonly ILogger<ModuleEventHandler> _logger;

        public ModuleEventHandler(ILogger<ModuleEventHandler> logger)
        {
            _logger = logger;
        }

        public void Handle(ContentSavingNotification notification)
        {
            foreach (var content in notification.SavedEntities)
            {
                if (content.ContentType.Alias == "module")
                {
                    try
                    {
                        if (string.IsNullOrEmpty(content.GetValue<string>("moduleId")))
                        {
                            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            var moduleId = $"{content.Name.ToLower().Replace(" ", "-")}_{timestamp}";
                            content.SetValue("moduleId", moduleId);
                        }

                        if (string.IsNullOrEmpty(content.GetValue<string>("version")))
                        {
                            var version = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                            content.SetValue("version", version.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in ModuleEventHandler while processing content {ContentId}", content.Id);
                    }
                }
            }
        }
    }
}
