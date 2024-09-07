namespace ElectricityRevitPlugin.UI;

public interface INotificationService
{
    void ShowNotification(string message);

    void ShowError(string title, string message);

}
