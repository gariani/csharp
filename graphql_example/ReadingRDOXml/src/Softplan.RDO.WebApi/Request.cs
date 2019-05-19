using System;
using System.ComponentModel;
using System.Net.Http;


namespace ConsoleApp1
{
    class Request: IDisposable
    {
        private bool disposed = false;
        private IntPtr handle;
        private Component component = new Component();
        private const string charset = "charset";
        private const string auth = "Authorization";
        private const string xSpDate = "x-sp-date";
        private const string xSpHash = "x-sp-hashAlgorithm";
        private HttpClient httpClient;
        private HttpRequestMessage request;

        public Request(string url)
        {
            httpClient = new HttpClient();
            request = new HttpRequestMessage(HttpMethod.Post, url);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {            
            if (!disposed)
            {
                if (disposing)
                {
                    component.Dispose();
                }

                CloseHandle(handle);
                handle = IntPtr.Zero;

                disposed = true;

            }
        }

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        ~Request()
        {
            Dispose(false);
        }

        public HttpResponseMessage RequestIt()
        {
            return null;
            // try
            // { 
                // httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/form-data; charset=utf-8");
                // var authorization = Canonic.GetCanonicalAuthorization();
                // request.Headers.Add(charset, Canonic.TypeEncode);
                // request.Headers.Add(auth, authorization);
                // request.Headers.Add(xSpDate, Canonic.SpData);
                // request.Headers.Add(xSpHash, Canonic.SpHash);
                // request.Content = Multi.GetMultipart();
                // var save = "data:" + Canonic.SpData + "\n\n" + "authorization:" + authorization;                
                // return httpClient.SendAsync(request).Result;
            // }
            // finally
            // {
            //     httpClient.CancelPendingRequests();
            // }
        }

    }
}
