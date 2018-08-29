using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.ApplicationInsights.Ingestion.Maestro
{
    public delegate ProcessingError EnvelopeProcessor(Contracts.Envelope envelope);

    /** Helper container of AI envelope batches to ensure order stays locked  */
    public class ProcessingBatch
    {
        private ProcessingEnvelope[] envelopes;

        public ProcessingBatch(Contracts.Envelope[] batch)
        {
            if (batch == null)
            {
                envelopes = new ProcessingEnvelope[0];
                return;
            }

            envelopes = new ProcessingEnvelope[batch.Length];
            for(int i = 0; i < batch.Length; i++)
            {
                envelopes[i] = new ProcessingEnvelope { data = batch[i] };
            }
        }

        /** 
         * Run a supplied EnvelopeProcessor delegate on every valid envelope in the batch
         * The supplied delegate should return a ProcessingError object to mark the envelope invalid.
         */
        public void Process(EnvelopeProcessor processor)
        {
            foreach(ProcessingEnvelope envelope in envelopes)
            {
                if (envelope.data != null && envelope.error != null)
                {
                    envelope.error = processor(envelope.data);
                }
                else if (envelope.data == null)
                {
                    envelope.error = new ProcessingError(StatusCodes.Status400BadRequest);
                }
            }
        }

        /** 
         * Generate a response conforming to the contract with SDKs for this batch, confirming
         * receipt and reporting errors.
         */
        public Contracts.SDKResponse GenerateResponse()
        {
            List<Contracts.SDKResponseError> errors = new List<Contracts.SDKResponseError>();

            for(int i = 0; i < envelopes.Length; i++)
            {
                if (envelopes[i].error != null)
                {
                    ProcessingError error = envelopes[i].error;
                    errors.Add(new Contracts.SDKResponseError
                    {
                        index = i,
                        statusCode = error.StatusCode,
                        message = error.Message
                    });
                }
            }

            Contracts.SDKResponseError[] errorArray = errors.ToArray();

            return new Contracts.SDKResponse
            {
                itemsReceived = envelopes.Length,
                itemsAccepted = envelopes.Length - errorArray.Length,
                errors = errorArray
            };
        }
    }
}