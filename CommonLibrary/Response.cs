using System;
using System.Text.Json.Serialization;
namespace CommonLibrary
{
    [Serializable]
    public class Response
    {
        #region properties
        public object Data { get; set; }

        public System.Net.HttpStatusCode Status { get; set; }

        public string ErrorId {  get; set; }

        [JsonIgnore]
        public Exception Exceptions { get; set; }
        #endregion;

        #region constructions
        public Response()
        {
            this.Status = System.Net.HttpStatusCode.OK;
        }

        public Response(object Data)
        {
            this.Data = Data;
            this.Status = System.Net.HttpStatusCode.OK;
        }

        public Response(string ErrorId, Exception Exceptions)
        {
            this.ErrorId = ErrorId;
            this.Exceptions = Exceptions;
            this.Status = System.Net.HttpStatusCode.InternalServerError;
        }
        #endregion
    }
}