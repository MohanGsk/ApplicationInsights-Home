using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    /** Endpoint that returns selected data from the profile matching a supplied iKey */
    public class ProfileEndpoint: BaseEndpoint 
    {
        private GraphDataClient dataClient;

        public ProfileEndpoint(GraphDataClient dataClient)
        {
            this.dataClient = dataClient;
        }

        public override async Task HandleGET(HttpContext context) 
        {
            string iKey = context.GetRouteParameter("iKey");
            string profileKey = context.GetRouteParameter("profileKey");

            Contracts.Profile profile = await dataClient.GetProfile(iKey);

            if (profile != null)
            {
                // Generate the exposable profile
                var exposableProfile = new Dictionary<string, object>();
                exposableProfile.Add("appId", profile.AppId);
                
                
                if (profileKey != null)
                {
                    // Respond with just the desired profile key
                    if (exposableProfile.ContainsKey(profileKey))
                    {
                        await Respond(context, StatusCodes.Status200OK, exposableProfile[profileKey]);
                        return;
                    }
                    else
                    {
                        await Respond(context, StatusCodes.Status404NotFound, "Profile Property Not Found");
                        return;
                    }
                }
                else
                {
                    // Respond with the exposable profile
                    await Respond(context, StatusCodes.Status200OK, exposableProfile);
                    return;
                }
            }
            
            // Looks like we didn't find anything
            await Respond(context, StatusCodes.Status404NotFound, "Profile Not Found");
        }
    }
}