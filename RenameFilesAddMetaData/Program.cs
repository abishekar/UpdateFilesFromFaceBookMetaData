using System;
using System.Collections.Generic;
using System.IO;

namespace RenameFilesAddMetaData
{
    /// <summary>
    /// This code depends on Facebook downloaded personal information JSON format.
    /// Code may need to be tweaked as and when the JSON structure changes 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            List<Photo> photos = new List<Photo>();
            List<Video> videos = new List<Video>();

            Console.WriteLine("Start reading JSON");
          

            var PJSON = System.IO.File.ReadAllText(@"D:\FBDataJSON\posts\your_posts_1.json");
            var VJSON = System.IO.File.ReadAllText(@"D:\FBDataJSON\photos_and_videos\your_videos.json");
            dynamic photojsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(PJSON);
            dynamic videojsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(VJSON);
            int cnt = 1;

            //read photos JSON and build photos list
            
            foreach (var item in photojsonObj)
            {
                if (item.attachments !=null)
                if(item.attachments.Count > 0)
                if (item.attachments[0].data  !=null)
                foreach (var ditem in item.attachments[0].data)
                {
                            if (ditem["media"] != null
                                 )
                            {
                                Console.WriteLine(cnt.ToString() +
                                    " : " + ditem["media"]["uri"] +" || created date:" 
                                    + new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.Parse(ditem["media"]["creation_timestamp"].ToString())));
                                    photos.Add(new Photo(ditem["media"]["uri"],
                                        new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.Parse(ditem["media"]["creation_timestamp"].ToString()))));
                                    cnt++;
                            }
                }
            }

            //read videos json & build video list
            
            foreach (var item in videojsonObj["videos"])
            {
                if (item.uri != null)
                               
                                    Console.WriteLine(cnt.ToString() +
                                        " : " + item["uri"] + " || created date:"
                                        + new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.Parse(item["creation_timestamp"].ToString())));
                                    videos.Add(new Video(item["uri"],
                                        new System.DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Int32.Parse(item["creation_timestamp"].ToString()))));
                                    cnt++;
            }

            //update photos
            foreach(Photo pt in photos)
            {

                if (File.Exists(@"D:\\FBDataJSON\\" + pt.uri))
                {
                    File.SetCreationTime(@"D:\\FBDataJSON\\" + pt.uri, pt.createdOn);
                    File.SetLastWriteTime(@"D:\\FBDataJSON\\" + pt.uri, pt.createdOn);
                    Console.WriteLine("Updated Photo File: ~ " + pt.uri);
                }

            }



            // update video files

            foreach (Video vs in videos)
            {
                if (File.Exists(@"D:\\FBDataJSON\\" + vs.uri))
                {
                    File.SetCreationTime(@"D:\\FBDataJSON\\" + vs.uri, vs.createdOn);
                    File.SetLastWriteTime(@"D:\\FBDataJSON\\" + vs.uri, vs.createdOn);
                    Console.WriteLine("Updated Video File: ~ " + vs.uri);
                }

            }

            Console.ReadLine();

        }


        
    }

    public class Photo
    {
        public string uri;
        public DateTime createdOn;
       

        public Photo(dynamic dynamic1, dynamic dynamic2)
        {
            this.uri = dynamic1;
            this.createdOn = dynamic2;
        }
    }

    public class Video
    {
        public string uri;
        public DateTime createdOn;
       

        public Video(dynamic dynamic1, dynamic dynamic2)
        {
            this.uri = dynamic1;
            this.createdOn = dynamic2;
        }
    }
}
