using OpenCvSharp;
using System.Text;

internal static class Video
{
    public static void PlayVideo(string fileName)
    {
        using var capture = new VideoCapture(fileName);

        double videoFps = capture.Fps;

        using var resizedFrame = new Mat();

        using var grayscaleFrame = new Mat();

        using var frame = new Mat();
        var lastFrameTime = DateTime.Now;
        while (capture.Read(frame))
        {
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight - 1;

            if (frame.Empty())
                break;

            Cv2.Resize(frame, resizedFrame, new Size(consoleWidth, consoleHeight));

            Cv2.CvtColor(resizedFrame, grayscaleFrame, ColorConversionCodes.BGR2GRAY);

            RenderFrame(grayscaleFrame, consoleWidth, consoleHeight);

            Thread.Sleep((int)(1000 / videoFps));
        }
    }

    private static void RenderFrame(Mat frame, int consoleWidth, int consoleHeight)
    {
        StringBuilder consoleOutput = new();

        int frameWidth = frame.Width;
        int frameHeight = frame.Height;

        for (int y = 0; y < consoleHeight; y++)
        {
            int frameY = y * frameHeight / consoleHeight;

            for (int x = 0; x < consoleWidth; x++)
            {
                int frameX = x * frameWidth / consoleWidth;
                byte pixelValue = frame.Get<byte>(frameY, frameX);

                // Map pixel value to a character (you can define your own mapping)
                char pixelCharacter = MapPixelValueToCharacter(pixelValue);

                consoleOutput.Append(pixelCharacter);
            }

            consoleOutput.Append('\n');
        }

        Console.Clear();
        Console.Write(consoleOutput.ToString());
    }

    static char MapPixelValueToCharacter(byte pixelValue)
    {
        // Define your mapping here; this is a simple example
        char[] densityChars = { ' ', '.', ':', 'o', 'O', 'X', '#', '@', '%' };
        int numChars = densityChars.Length;

        // Map pixel value to an index in the densityChars array
        int index = (int)(pixelValue / 255.0 * (numChars - 1));

        return densityChars[index];
    }
}
