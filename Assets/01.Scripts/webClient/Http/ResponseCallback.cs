using UnityEngine;
using System;
using System.Collections;

namespace HTTP
{
	public class ResponseCallback : MonoBehaviour
    {
        private static ResponseCallback singleton = null;
        private static GameObject singletonGameObject = null;
        private static object singletonLock = new object();

        public static ResponseCallback Singleton
        {
            get {
                return singleton;
            }
        }

        public Queue requests = Queue.Synchronized( new Queue() );

        public static void Init()
        {
            if ( singleton != null )
            {
                return;
            }

            lock( singletonLock )
            {
                if ( singleton != null )
                {
                    return;
                }

                singletonGameObject = new GameObject();
                singleton = singletonGameObject.AddComponent<ResponseCallback>();
                singletonGameObject.name = "HTTPResponseCallback";
            }
        }

        public void Update()
        {
            while( requests.Count > 0 )
            {
                HTTP.Request request = (Request)requests.Dequeue();
                request.completedCallback( request );
            }
        }
    }
}
