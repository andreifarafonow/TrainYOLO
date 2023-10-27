using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsolePrepareData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] pathsVideo = {@"D:\train_dataset_dataset\video0\", @"D:\train_dataset_dataset\video1\", @"D:\train_dataset_dataset\video2\"};
            string pathToRgb = @"frames_rgb\";

            string pathOutput = @"C:\Users\AndreyUser\Desktop\Train\DataToTrain\";


            List<PictureTest> pictures = new List<PictureTest>();

            foreach (var pathVideo in pathsVideo)
            {
                var files = Directory.GetFiles(pathVideo + pathToRgb);

                var a = pathVideo.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries).Last();

                var pictureDiscriptionsStrings = File.ReadAllLines(pathVideo + a + ".txt").Select(x=> x.Replace(" ", ""));

                List<string[]> pictureDiscriptions = new List<string[]>(); 

                foreach (var pictureDiscription in pictureDiscriptionsStrings)
                {
                    pictureDiscriptions.Add(pictureDiscription.Split(','));
                }

                foreach (var file in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(file);

                    var letNumber = int.Parse(fileName);

                    var descByPicture = pictureDiscriptions.Where(x => x[0] == letNumber.ToString());

                    List<string> resultByImmage = descByPicture.Select(x => x.Skip(2)).Select(x => string.Join(" ", x)).ToList(); 

                    pictures.Add(new PictureTest { PathPicture = file, Description = resultByImmage});
                }
            }

            var mixedPictures = pictures.OrderBy(x => x.GetHashCode());

            int NameId = 0;

            var train = @"train\";
            foreach (var item in mixedPictures.Take(800))
            {
                CreateTestData(pathOutput + train, item);
            }

            var valid = @"test\";
            foreach (var item in mixedPictures.Skip(800).Take(210))
            {
                CreateTestData(pathOutput + valid, item);
            }

            CreateTestData(pathOutput + @"valid\", mixedPictures.Last());


            void CreateTestData (string path, PictureTest pictureTest)
            {
                /*
                 Преобразование PNG to JPG
                 */

                var nameImage = NameId++;

                string sourceImagePath = pictureTest.PathPicture;
                string destinationImagePath = path + "images\\" + nameImage + ".jpg";

                Directory.CreateDirectory(Path.GetDirectoryName(destinationImagePath));
                using (Image image = Image.FromFile(sourceImagePath))
                {
                    image.Save(destinationImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                var labelDestanation = path + "labels\\" + nameImage + ".txt";
                Directory.CreateDirectory(Path.GetDirectoryName(labelDestanation));
                File.WriteAllLines(labelDestanation, pictureTest.Description);


            }
        }

        

    }

    class PictureTest
    {
        public string PathPicture { get; set; }
        public List<string> Description { get; set; }
    }
}
