    using System;
    using System.IO;
    using Android.OS;
    using ArtScanner.Services;
    using Java.IO;
    using Java.Net;
    using Environment = System.Environment;

    namespace ArtScanner.Droid
    {
        public class BackgroundAudioFileAsyncTask : AsyncTask<string, string, string>
        {
            FileOutputStream fileOutputStream;
            Stream inputStream;

            protected override string RunInBackground(params string[] @params)
            {
                var musicLink = @params[0].ToString();
            
                var documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
                string localFilename = @params[1].ToString();
                string localPath = Path.Combine(documentsPath, localFilename);

                try
                {

                    URL url = new URL(musicLink);
              
                    inputStream = url.OpenStream();

                    fileOutputStream = new FileOutputStream(localPath);
                

                    int c;
                    byte[] buffer = new byte[8192];

                    while ((c = inputStream.Read(buffer)) != -1)
                    {
                        fileOutputStream.Write(buffer, 0, c);
                    }

                    //URL url = new URL(@params[0]);
                    //URLConnection connection = url.OpenConnection();
                    //connection.Connect();
                    //int LengthOfFile = connection.ContentLength;
                    //InputStream input = new BufferedInputStream(url.OpenStream(), LengthOfFile);
                    //OutputStream output = new FileOutputStream(localPath);

                    //byte[] data = new byte[1024];
                    //long total = 0;
                    //while ((count = input.Read(data)) != -1)
                    //{
                    //    total += count;
                    //    output.Write(data, 0, count);
                    //}

                    fileOutputStream.Flush();
                    fileOutputStream.Close();
                    inputStream.Close();
                }

                catch (Exception ex)
                {
                    LogService.Log(ex);
                }


                return null;
            }

            protected override void OnPreExecute()
            {
                LogService.Log(null,"Downloading file ...");
                base.OnPreExecute();
            }


            protected override void OnProgressUpdate(params string[] values)
            {
                LogService.Log(null, (int.Parse(values[0])).ToString());
                base.OnProgressUpdate(values);
            }
            protected override void OnPostExecute(string result)
            { 
                LogService.Log(null, "Download completed !");

                base.OnPostExecute(result);
            }

            protected override void OnCancelled()
            {
                base.OnCancelled();

                fileOutputStream?.Close();
                inputStream?.Close();
            
            }
        }
    }
