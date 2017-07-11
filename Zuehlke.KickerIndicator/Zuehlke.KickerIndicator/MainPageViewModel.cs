using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Humanizer;

namespace Zuehlke.KickerIndicator
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public MainPageViewModel()
        {
            Check();
        }

        private async void Check()
        {
            // 

            using (var webClient = new WebClient())
            {
                try
                {
                    var jsonString = await webClient.DownloadStringTaskAsync("https://zkicker.cerritus.eu/current-status.json");
                    var json = JObject.Parse(jsonString);
                    // { "Kicker" : { "occupied" : 0, "sinceUTC" : "2017-06-28T19:07:34Z" } }

                    var wasFree = IsFree;
                    IsOccupied = json["Kicker"]["occupied"].ToString() == "1";

                    var sinceUtc = json["Kicker"]["sinceUTC"].ToString();
                    var since = System.DateTime.Parse(sinceUtc);
                    var now = System.DateTime.UtcNow;
                    Text = $"Der Kicker ist {(IsFree ? "frei" : "belegt")} seit {(now - since).Humanize(precision: 2)}";

                    OnPropertyChanged("Text");
                    OnPropertyChanged("IsOccupied");
                    OnPropertyChanged("IsFree");

                    if (IsFree != wasFree)
                    {
                        Plugin.Vibrate.CrossVibrate.Current.Vibration(1000);
                    }

                }
                catch
                {
                }
                await Task.Delay(10000);
                Check();
            }
        }

        public string Text { get; private set; }
        public bool IsOccupied { get; private set; }
        public bool IsFree => !IsOccupied;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}