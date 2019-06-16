using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Noisera.Domain;
using Noisera.Infrastructure;
using Amazon.Lambda.Core;
using System.Net.Http;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace UpdateTracksOrder
{
    public class UpdateTracksOrder
    {
        private GigTrackDTO parserInputToObject(JObject input, string gigGUID)
        {
            return new GigTrackDTO(
                    (bool)input["Active"],
                    (int)input["Order"],
                    gigGUID,
                    (string)input["GUID"],
                    (string)input["Observations"]
                    );
        }

        private List<string> getColumnsToFilter()
        {
            List<string> columnsToFilter = new List<string>();

            columnsToFilter.Add("GigGUID");
            columnsToFilter.Add("TrackGUID");

            return columnsToFilter;
        }

        public HttpResponseMessage UpdateTracksOrderHandler(JObject input){
            string gigGUID = (string)input["gigGUID"];
            JArray tracks = (JArray)input["tracks"];
            foreach (JObject track in tracks)
            {
                GigTrackDTO trackToUpdate = parserInputToObject(track, gigGUID);
                List<string> columnsToFilter = getColumnsToFilter();
                GenericNoiseraDatabase.Update("tracks_gig", trackToUpdate, columnsToFilter);
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Created);
        }
    }
}
