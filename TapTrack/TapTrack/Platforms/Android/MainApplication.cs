using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;

namespace TapTrack.Platforms.Android
{
    [global::Android.App.Application]
    public class MainApplication : global::Android.App.Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}

namespace TapTrack
{
    [BroadcastReceiver(Enabled = true, Exported = true, Name = "com.companyname.taptrack.CarTapReceiver")]
    [IntentFilter(new[] { "com.companyname.taptrack.CAR_TAP_ACTION" })]
    [Preserve(AllMembers = true)]
    public class CarTapReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (intent == null) return;

            try
            {
                var label = intent.GetStringExtra("label");
                var timestampExtra = intent.GetStringExtra("timestamp");

                DateTimeOffset timestamp = DateTimeOffset.Now;
                if (!string.IsNullOrEmpty(timestampExtra))
                {
                    if (long.TryParse(timestampExtra, out var ticks))
                    {
                        timestamp = new DateTimeOffset(ticks, TimeSpan.Zero);
                    }
                    else if (DateTimeOffset.TryParse(timestampExtra, out var parsed))
                    {
                        timestamp = parsed;
                    }
                }

                var message = $"Received tap: '{label ?? "(null)"}' at {timestamp:O}";
                // Log for debug
                Log.Info("TapTrack", message);

                // Show a short Toast so you can visually confirm receipt while testing
                if (context != null)
                {
                    Toast.MakeText(context, message, ToastLength.Short)?.Show();
                }

                // Fire-and-forget send
                _ = SendTapToApiAsync(label, timestamp);
            }
            catch (Exception ex)
            {
                Log.Error("TapTrack", ex.ToString());
            }
        }

        private async Task SendTapToApiAsync(string? label, DateTimeOffset timestamp)
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Accept.Clear();
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var payload = new { Label = label ?? string.Empty, Timestamp = timestamp };
                var json = JsonSerializer.Serialize(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");

                // TODO: replace with your real endpoint
                var endpoint = "https://example.com/api/taps";
                await http.PostAsync(endpoint, content).ConfigureAwait(false);
            }
            catch
            {
                // Swallow errors here; consider logging to file or reporting via platform logger
            }
        }
    }
}
