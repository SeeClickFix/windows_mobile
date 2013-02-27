using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeClickFix.WP8.SeeClickFixAPI
{
    public static class RestClientExtensions
    {
        public static Task<T> ExecuteTaskAsync<T>(this RestClient client, RestRequest request) where T : new()
        {
            if (client == null)
            {
                throw new NullReferenceException();
            }

            var tcs = new TaskCompletionSource<T>();
            client.ExecuteAsync<T>(request, (response) =>
            {
                // If there is a network transport error (network is down, failed DNS lookup, etc), 
                //   RestResponse.Status will be set to ResponseStatus.Error, 
                //   otherwise it will be ResponseStatus.Completed. 
                // If an API returns a 404, ResponseStatus will still be Completed. 
                // If you need access to the HTTP status code returned you will find it at 
                //    RestResponse.StatusCode. 
                //    The Status property is an indicator of completion independent of the API error handling.

                //if (response.ResponseStatus == ResponseStatus.Completed && 
                //    response.StatusCode == System.Net.HttpStatusCode.OK)
                //{

                //}

               // var result = tcs.Task.Result;
                if (response != null && response.ErrorException != null)
                {
                    tcs.TrySetException(response.ErrorException);
                }
                else
                {
                    tcs.TrySetResult(response != null ? response.Data : default(T));
                }
            });
            return tcs.Task;
        }
    }
}
