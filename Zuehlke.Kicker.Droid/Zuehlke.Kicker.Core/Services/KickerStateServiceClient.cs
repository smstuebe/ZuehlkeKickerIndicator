using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zuehlke.Kicker.Core.Services
{
    public class KickerStateServiceClient : IKnowTheKickerState
    {
        public class KickerServiceModel
        {
            public KickerStateServiceModel Kicker { get; set; }
        }

        public class KickerStateServiceModel
        {
            public int occupied { get; set; }
            public DateTime sinceUTC { get; set; }
        }

        private const string URL = "http://zkicker.textmo.de/current-status.json";

        public Task<KickerState> Current()
        {
            var taskCompletionSource = new TaskCompletionSource<KickerState>();
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.BeginGetResponse(o =>
                {
                    try
                    {
                        var result = request.EndGetResponse(o);
                        var serializer = new JsonSerializer();

                        using (var responseStream = result.GetResponseStream())
                        {
                            using (var textReader = new StreamReader(responseStream))
                            {
                                using (var jsonReader = new JsonTextReader(textReader))
                                {
                                    var model = serializer.Deserialize<KickerServiceModel>(jsonReader);
                                    var state = new KickerState(model.Kicker.occupied == 0, model.Kicker.sinceUTC.ToLocalTime());
                                    taskCompletionSource.SetResult(state);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        taskCompletionSource.SetResult(null);
                    }
                },
                null);
            }
            catch (Exception e)
            {
                taskCompletionSource.SetResult(null);
            }

            return taskCompletionSource.Task;
        }
    }
}
