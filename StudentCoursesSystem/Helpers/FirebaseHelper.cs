using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.Helpers
{
  public static class FirebaseHelper
  {
    private static readonly HttpClient client = new HttpClient();
    //private static readonly string firebaseServerKey = "YOUR_FIREBASE_SERVER_KEY"; // Replace with your actual Firebase server key
    private static readonly string firebaseUrl = "https://fcm.googleapis.com/fcm/send";

    public static async Task SendPushNotificationToTopicAsync(string topic, string title, string body)
    {
      var notification = new
      {
        to = $"/topics/{topic}",
        notification = new
        {
          title,
          body
        }
      };

      var json = JsonSerializer.Serialize(notification);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      //client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"key={firebaseServerKey}");
      
      try
      {
        var response = await client.PostAsync(firebaseUrl, content);
        response.EnsureSuccessStatusCode();
      }
      catch (Exception ex)
      {
        // Log the error
        Console.WriteLine($"Error sending notification: {ex.Message}");
      }
    }
  }
}