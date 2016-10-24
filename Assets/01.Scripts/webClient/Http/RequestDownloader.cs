using UnityEngine;
using System.Collections;
using System.IO;
using System;
using HTTP;

namespace HTTP
{
	public class ReauestStatus
	{
		public bool isDone = false;
		public bool fromCache = false;
		public Request request = null;
	}


    public class RequestDownloader : MonoBehaviour
	{
		string mCachePath = null;

		static RequestDownloader mInstance = null;
        public static RequestDownloader Instance
        {
			get {
                if (mInstance == null)
                {
                    var g = new GameObject("DiskCache", typeof(RequestDownloader));
					g.hideFlags = HideFlags.HideAndDontSave;
                    mInstance = g.GetComponent<RequestDownloader>();
				}
                return mInstance;
			}
		}

		void Awake ()
		{
            mCachePath = System.IO.Path.Combine(GetCacheFilePath(), "cacheIamge");
            if (!Directory.Exists(mCachePath))
                Directory.CreateDirectory(mCachePath);
		}

        public void CreateFolder(string folderName)
        {
            mCachePath = System.IO.Path.Combine(GetCacheFilePath(), folderName );
            if (!Directory.Exists(mCachePath))
                Directory.CreateDirectory(mCachePath);

        }


		public ReauestStatus Fetch (Request request, string name)
		{
            var filename = System.IO.Path.Combine(mCachePath, name);
            if (File.Exists(filename) && File.Exists(filename + ".etag"))
            {
                request.SetHeader("If-None-Match", File.ReadAllText(filename + ".etag"));
            }
			var handle = new ReauestStatus ();
			handle.request = request;
			StartCoroutine (DownloadAndSave (request, filename, handle));
			return handle;
		}

		IEnumerator DownloadAndSave (Request request, string filename, ReauestStatus handle)
		{
			var useCachedVersion = File.Exists(filename);
            Action< HTTP.Request > callback = request.completedCallback;
			request.Send(); // will clear the completedCallback
			while (!request.isDone)
				yield return new WaitForEndOfFrame ();
			if (request.exception == null && request.response != null) {
				if (request.response.status == 200) {
					var etag = request.response.GetHeader ("etag");
					if (etag != "") {
						File.WriteAllBytes (filename, request.response.bytes);
						File.WriteAllText (filename + ".etag", etag);
					}
					useCachedVersion = false;
				}
			}

			if(useCachedVersion) {
				if(request.exception != null) {
					request.exception = null;
				}
				request.response.status = 304;
				request.response.bytes = File.ReadAllBytes (filename);
				request.isDone = true;
			}
			handle.isDone = true;

            if ( callback != null )
            {
                callback( request );
            }
		}

        public string GetCacheFilePath()
        {
            string path;
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
                path = path.Substring(0, path.LastIndexOf('/'));
                
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                path = Application.persistentDataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                
            }
            else
            {
                path = Application.dataPath;
                path = path.Substring(0, path.LastIndexOf('/'));
                
            }
            return path;
        }

        public void DeleteAllFiles()
        {
            Directory.Delete(mCachePath, true);

        }

	}

   

}
