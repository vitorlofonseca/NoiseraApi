using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Noisera.Domain;
using Noisera.Infrastructure;
using Amazon.Lambda.Core;
using System.Net.Http;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace SaveTrackAWS
{
    public class SaveTrackAWS
    {

        private List<string> getColumnsToFilter()
        {
            List<string> columnsToFilter = new List<string>();

            columnsToFilter.Add("GigGUID");
            columnsToFilter.Add("TrackGUID");

            return columnsToFilter;
        }

        private GigTrackDTO parserInputToObject(JObject input)
        {
            return new GigTrackDTO(
                    (bool)input["Active"],
                    (int)input["Order"],
                    (string)input["gigGUID"],
                    (string)input["GUID"],
                    (string)input["Observations"]
                    );
        }

        public HttpResponseMessage SaveTrackHandler(JObject input)
        {
            Track track = input.ToObject<Track>();
            string guidOfStoredTrack = TrackNoiseraDatabase.getGuidOfStoredTrack(track);
            string test = (string)input["gigGUID"];

            if (guidOfStoredTrack == null)
            {
                throw new Exception("Implement this method");
            }
            else
            {
                GigTrackDTO trackToUpdate = parserInputToObject(input);
                List<string> columnsToFilter = getColumnsToFilter();

                GenericNoiseraDatabase.Update("tracks_gig", trackToUpdate, columnsToFilter);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }
    }
}
