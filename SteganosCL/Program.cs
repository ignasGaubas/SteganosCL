using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace SteganosCL
{
    internal class Program
    {
        private string _sourceImagePath;
        private Bitmap _sourceImage;
        private string _message;
        private string _targetImagePath;
        private Bitmap _encodedImage;

        private static void Main(string[] args)
        {
            try
            {
                Program program = new Program();
                program.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured:\n{e.Message}");
                throw;
            }
        }

        private void Run()
        {
            Console.WriteLine("This is SteganosCL - command-line too for Windows, that encodes and decodes your text into images.");

            Console.WriteLine("Please specify an image, into which the message will be encoded.");
            Console.WriteLine("Source image path:");

            _sourceImagePath = ReadSourceImagePath();
            _sourceImage = ReadImage(_sourceImagePath);

            Console.WriteLine($"{_sourceImagePath}");

            Console.WriteLine("Please specify the path, where result image will be written.");
            Console.WriteLine("Result image path:");

            _targetImagePath = ReadTargetImagePath();

            Console.WriteLine($"{_targetImagePath}");

            Console.WriteLine("Please write your message that you wish encoded below.");
            //Console.WriteLine($"Your message should not be longer than {10} characters.");

            _message = ReadMessage();

            _encodedImage = EncodeMessage(_sourceImage, _message);

            SaveImage(_encodedImage, _targetImagePath);
        }

        /// <summary>
        /// Reads file path from console. If file at given path does not exist,
        /// throws <see cref="FileNotFoundException"/>.
        /// </summary>
        /// <returns></returns>
        private string ReadSourceImagePath()
        {
            string imagePath = Console.ReadLine();
            bool fileExists = File.Exists(imagePath);
            if (!fileExists)
                throw new FileNotFoundException($"File does not exist: {imagePath}");

            return imagePath;
        }

        /// <summary>
        /// Reads file path from prompt window. If file at given path does not exist,
        /// throws <see cref="FileNotFoundException"/>.
        /// </summary>
        /// <returns></returns>
        private string ReadSourceImagePath2()
        {
            OpenFileDialog prompt = new OpenFileDialog();
            DialogResult result = prompt.ShowDialog();
            string imagePath = prompt.FileName;
            bool fileExists = File.Exists(imagePath);
            if (!fileExists)
                throw new FileNotFoundException($"File does not exist: {imagePath}");

            return imagePath;
        }

        /// <summary>
        /// Reads target file path from console window.
        /// </summary>
        /// <returns></returns>
        private string ReadTargetImagePath()
        {
            string imagePath = Console.ReadLine();
            //bool fileExists = File.Exists(imagePath);
            //if (!fileExists)
            //    throw new FileNotFoundException($"File does not exist: {imagePath}");

            return imagePath;
        }

        /// <summary>
        /// Reads target file path from prompt window.
        /// </summary>
        /// <returns></returns>
        private string ReadTargetImagePath2()
        {
            SaveFileDialog prompt = new SaveFileDialog();
            DialogResult result = prompt.ShowDialog();
            string imagePath = prompt.FileName;
            //bool fileExists = File.Exists(imagePath);
            //if (!fileExists)
            //    throw new FileNotFoundException($"File does not exist: {imagePath}");

            return imagePath;
        }

        /// <summary>
        /// Reads user message from console. If message is not entered,
        /// throws <see cref="InvalidDataException"/>.
        /// </summary>
        /// <returns></returns>
        private string ReadMessage()
        {
            string message = Console.ReadLine();
            bool isMessageValid = !string.IsNullOrEmpty(message);
            if (!isMessageValid)
                throw new InvalidDataException($"Message invalid: {message}");

            return message;
        }

        /// <summary>
        /// Reads image from file.
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private Bitmap ReadImage(string imagePath)
        {
            Bitmap image = (Bitmap)Bitmap.FromFile(imagePath);
            return image;
        }

        /// <summary>
        /// Encodes given message string into given image.
        /// Each char of message is encoded into each pixels red component.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Bitmap EncodeMessage(Bitmap source, string message)
        {
            int i = 0;
            Bitmap result = (Bitmap)source.Clone();

            for (int row = 0; row < source.Width; row++)
            {
                for (int col = 0; col < source.Height; col++)
                {
                    Color pixel = source.GetPixel(row, col);
                    Color newPixel = AddCharToPixel(pixel, message[i++]);
                    result.SetPixel(row, col, newPixel);

                    if (i >= message.Length)
                        break;
                }
                if (i >= message.Length)
                    break;
            }

            return result;
        }

        /// <summary>
        /// Replaces red component of pixel with byte value of given character.
        /// </summary>
        /// <param name="pixel"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        private Color AddCharToPixel(Color pixel, char symbol)
        {
            byte symByte = (byte)symbol;
            Color newColor = Color.FromArgb(pixel.A, symByte, pixel.G, pixel.B);
            return newColor;
        }

        /// <summary>
        /// Saves image to given path.
        /// </summary>
        /// <param name="encodedImage"></param>
        /// <param name="targetPath"></param>
        private void SaveImage(Bitmap encodedImage, string targetPath)
        {
            encodedImage.Save(targetPath, ImageFormat.Jpeg);
        }
    }
}
