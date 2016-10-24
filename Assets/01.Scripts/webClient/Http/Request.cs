using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Globalization;
using System.Threading;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace HTTP
{
	public class HTTPException : Exception
	{
		public HTTPException (string message) : base(message)
		{
		}
	}

	public enum RequestState {
		Waiting, Reading, Done
	}

	public class Request
	{
        public static bool mIsLogAllRequests = false;
        public static bool mIsVerboseLogging = false;
        //Http cookie Manage
        public CookieMananager cookieMgr = CookieMananager.Instance;
		public string mMethod = "GET";
		public string mProtocol = "HTTP/1.1";
		public byte[] mBytes;
		public Uri uri;
		public static byte[] EOL = { (byte)'\r', (byte)'\n' };
		public Response response = null;
		public bool isDone = false;
		public int maximumRetryCount = 3;
		public bool acceptGzip = true;
		public bool useCache = false;
        public int useCookie = 1;
		public Exception exception = null;
		public RequestState state = RequestState.Waiting;
        public long responseTime = 0; // in milliseconds
		public bool synchronous = false;
        private readonly int TIME_OUT = 3000;

		public Action< HTTP.Request > completedCallback = null;

		Dictionary<string, List<string>> headers = new Dictionary<string, List<string>> ();
		static Dictionary<string, string> etags = new Dictionary<string, string> ();

		public Request (string method, string uri)
		{
            this.mMethod = method;
			this.uri = new Uri (uri);
		}

		public Request (string method, string uri, bool useCache)
		{
            this.mMethod = method;
			this.uri = new Uri (uri);
			this.useCache = useCache;
		}

		public Request (string method, string uri, byte[] bytes)
		{
            this.mMethod = method;
			this.uri = new Uri (uri);
            this.mBytes = bytes;
		}

      

        public Request( string method, string uri, WWWForm form )
        {
            this.mMethod = method;
			this.uri = new Uri (uri);
            this.mBytes = form.data;
            foreach ( DictionaryEntry entry in form.headers )
            {
                this.AddHeader( (string)entry.Key, (string)entry.Value );
            }
        }

        public Request(string method, string uri, WWWForm form, string authkey)
        {
            this.mMethod = method;
            UriBuilder ub = new UriBuilder(uri);
            ub.UserName = authkey;
            this.uri = ub.Uri;
            this.mBytes = form.data;
            foreach (DictionaryEntry entry in form.headers)
            {
                this.AddHeader((string)entry.Key, (string)entry.Value);
            }
        }

        public Request(string method, string uri, WWWForm form, int useCookie)
        {
            this.mMethod = method;
            this.uri = new Uri(uri);
            this.mBytes = form.data;
            this.useCookie = useCookie;
            foreach (DictionaryEntry entry in form.headers)
            {
                this.AddHeader((string)entry.Key, (string)entry.Value);
            }
        }

      

        public Request( string method, string uri, Hashtable data )
        {
            this.mMethod = method;
            this.uri = new Uri( uri );
            //this.mBytes = Encoding.UTF8.GetBytes(JSON.JsonEncode(data));
            this.AddHeader( "Content-Type", "application/json" );
        }
        
		public void AddHeader (string name, string value)
		{
			name = name.ToLower ().Trim ();
			value = value.Trim ();
			if (!headers.ContainsKey (name))
				headers[name] = new List<string> ();
			headers[name].Add (value);
		}

		public string GetHeader (string name)
		{
			name = name.ToLower ().Trim ();
			if (!headers.ContainsKey (name))
				return "";
			return headers[name][0];
		}

        public List< string > GetHeaders()
        {
            List< string > result = new List< string >();
            foreach (string name in headers.Keys) {
				foreach (string value in headers[name]) {
                    result.Add( name + ": " + value );
				}
			}

            return result;
        }

		public List<string> GetHeaders (string name)
		{
			name = name.ToLower ().Trim ();
			if (!headers.ContainsKey (name))
				headers[name] = new List<string> ();
			return headers[name];
		}

		public void SetHeader (string name, string value)
		{
			name = name.ToLower ().Trim ();
			value = value.Trim ();
			if (!headers.ContainsKey (name))
				headers[name] = new List<string> ();
			headers[name].Clear ();
			headers[name].TrimExcess();
			headers[name].Add (value);
		}

        
        public void Send()
        {
            Send( null );
        }

        private void GetResponse()
        {
            System.Diagnostics.Stopwatch curcall = new System.Diagnostics.Stopwatch();
            curcall.Start();


            var retry = 0;
            System.Threading.WaitHandle wh = null;
            IAsyncResult ar = null;
            TcpClient client = null;
            client = new TcpClient();

            response = new Response();
            do
            {
                try
                {
                    if (useCache)
                    {
                        string etag = "";
                        if (etags.TryGetValue(uri.AbsoluteUri, out etag))
                        {
                            SetHeader("If-None-Match", etag);
                        }
                    }
                    SetHeader("Host", uri.Host);
                    ar = client.BeginConnect(uri.Host, uri.Port, null, null);

                    wh = ar.AsyncWaitHandle;
                    if (!ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(TIME_OUT), false))
                    {
                        client.Close();
                        throw new TimeoutException();
                    }
                    client.EndConnect(ar);
                    client.ReceiveTimeout = TIME_OUT;
                    client.SendTimeout = TIME_OUT;
                    using (var stream = client.GetStream())
                    {
                        Stream ostream = stream as Stream;
                        if (uri.Scheme.ToLower() == "https")
                        {
                            ostream = new SslStream(stream, false, new RemoteCertificateValidationCallback(ValidateServerCertificate));
                            var ssl = ostream as SslStream;
                            ssl.ReadTimeout = TIME_OUT;
                            ssl.WriteTimeout = TIME_OUT;
                            ssl.AuthenticateAsClient(uri.Host);
                        }
                        WriteToStream(ostream);
                        
                        response.request = this;
                        state = RequestState.Reading;
                        response.ReadFromStream(ostream);
                    }
                    wh.Close();
                    client.Close();
					//Utility.LogWarning("respon.status = " +response.status);
                    switch (response.status)
					{

                        case 307:
                        case 302:
                        case 301:
                            uri = new Uri(response.GetHeader("Location"));
                            continue;
                        default:
                            retry = maximumRetryCount;

                            break;
                    }
                }
                catch (Exception e)
                {
                    if (wh != null)
                        wh.Close();
                    if (client != null)
                        client.Close();

                    exception = e;
                    response.status = 600;
                    System.Threading.Thread.Sleep(1000);
					//Utility.LogWarning("respon.status 2= " +response.status);
                }
                finally
                {
                    if (wh != null)
                        wh.Close();
                    if (client != null)
                        client.Close();
                }
                
            }while(++retry < maximumRetryCount);


            if (useCache)
            {
                string etag = response.GetHeader("etag");
                if (etag.Length > 0)
                    etags[uri.AbsoluteUri] = etag;
            }


            state = RequestState.Done;
            isDone = true;
            responseTime = curcall.ElapsedMilliseconds;

            if (completedCallback != null)
            {
                if (synchronous)
                {
                    completedCallback(this);
                }
                else
                {
                    ResponseCallback.Singleton.requests.Enqueue(this);
                }
            }

            if (mIsLogAllRequests)
            {
#if !UNITY_EDITOR
				//System.Console.WriteLine("NET: " + InfoString( VerboseLogging ));
#else
                if (response != null && response.status >= 200 && response.status < 300)
                {
                }
                else if (response != null && response.status >= 400)
                {
                }
                else
                {
                }
#endif
            }
        }
		
		public void Send( Action< HTTP.Request > callback )
		{

            if (!synchronous && callback != null && ResponseCallback.Singleton == null)
            {
                ResponseCallback.Init();
            }

            completedCallback = callback;

			isDone = false;
			state = RequestState.Waiting;
			if (acceptGzip)
				SetHeader ("Accept-Encoding", "gzip");

            if (this.cookieMgr != null && useCookie == 1)
			{
                List<Cookie> cookies = this.cookieMgr.GetCookies(new CookieAccessInfo(uri.Host, uri.AbsolutePath));
								string cookieString = this.GetHeader( "cookie" );
				for ( int cookieIndex = 0; cookieIndex < cookies.Count; ++cookieIndex )
				{
					if ( cookieString.Length > 0 && cookieString[ cookieString.Length - 1 ] != ';' )
					{
						cookieString += ';';
					}
					cookieString += cookies[ cookieIndex ].name + '=' + cookies[ cookieIndex ].value + ';';
				}
		        SetHeader( "cookie", cookieString );
		    }

            if (mBytes != null && mBytes.Length > 0 && GetHeader("Content-Length") == "")
            {
                SetHeader("Content-Length", mBytes.Length.ToString());
            }

            if ( GetHeader( "User-Agent" ) == "" ) {
                SetHeader( "User-Agent", "Unity " + Application.unityVersion );
                //SetHeader( "User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.155 Safari/537.36" );
            }

            if ( GetHeader( "Connection" ) == "" ) {
                SetHeader( "Connection", "close" );
            }
			
	
			if (!String.IsNullOrEmpty(uri.UserInfo)) {
                //Utility.Log(uri.UserInfo);
				SetHeader("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(uri.UserInfo)));
			}
			
			if (synchronous) {
				GetResponse();
			} else {
				ThreadPool.QueueUserWorkItem (new WaitCallback ( delegate(object t) {
					GetResponse();
				})); 
			}
		}

		public string Text {
            set { mBytes = System.Text.Encoding.UTF8.GetBytes(value); }
		}


		public static bool ValidateServerCertificate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
#if !UNITY_EDITOR
            System.Console.WriteLine( "NET: SSL Cert: " + sslPolicyErrors.ToString() );
#else
#endif
			return true;
		}

		void WriteToStream (Stream outputStream)
		{
			var stream = new BinaryWriter (outputStream);
            stream.Write(ASCIIEncoding.ASCII.GetBytes(mMethod.ToUpper() + " " + uri.PathAndQuery + " " + mProtocol));
			stream.Write (EOL);

			foreach (string name in headers.Keys) {
				foreach (string value in headers[name]) {
					stream.Write (ASCIIEncoding.ASCII.GetBytes (name));
					stream.Write (':');
					stream.Write (ASCIIEncoding.ASCII.GetBytes (value));
					stream.Write (EOL);
				}
			}

            stream.Write (EOL);

            if (mBytes != null && mBytes.Length > 0)
            {
                stream.Write(mBytes);
			}
		}

        private static string[] sizes = { "B", "KB", "MB", "GB" };
        public string InfoString( bool verbose )
        {
            string status = isDone && response != null ? response.status.ToString() : "---";
            string message = isDone && response != null ? response.message : "Unknown";
            double size = isDone && response != null && response.bytes != null ? response.bytes.Length : 0.0f;

            int order = 0;
            while ( size >= 1024.0f && order + 1 < sizes.Length )
            {
                ++order;
                size /= 1024.0f;
            }

            string sizeString = String.Format( "{0:0.##}{1}", size, sizes[ order ] );

            string result = uri.ToString() + " [ " + mMethod.ToUpper() + " ] [ " + status + " " + message + " ] [ " + sizeString + " ] [ " + responseTime + "ms ]";

            if ( verbose && response != null )
            {
                result += "\n\nRequest Headers:\n\n" + String.Join( "\n", GetHeaders().ToArray() );
                result += "\n\nResponse Headers:\n\n" + String.Join( "\n", response.GetHeaders().ToArray() );

                if ( response.Text != null )
                {
                    result += "\n\nResponse Body:\n" + response.Text;
                }
            }

            return result;
        }
	}
}
