if (args.Length != 1)
{
    Console.WriteLine("Invalid args, need 1 argument containing a path to a video file");
    return;
}

Video.PlayVideo(args[0]);