// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
int maxLength = 140;

if (args.Length == 0)
{
    var dir = new DirectoryInfo("/mnt/c/h");
    Findfiles(dir);
}

if (args.Length == 1)
{
    var dir = new DirectoryInfo(args[0]);
    Findfiles(dir);

}

if (args.Length == 2)
{
    var dir = new DirectoryInfo(args[0]);
    maxLength = int.Parse(args[1]);
    Findfiles(dir);

}


void Findfiles(DirectoryInfo directory)
{
    var infos = directory.GetFiles("*", SearchOption.AllDirectories);
    Console.WriteLine($"Got {infos.Length} files in {directory.FullName}");
    var longers = new List<FileInfo>();
    var renameFailed = new List<FileInfo>();
    foreach (var info in infos)
    {
        if (DoIfLonger(IsLongerThan, IsLonger, info))
            longers.Add(info);
    }

    Console.WriteLine($"Found {longers.Count} with long file names");

    foreach (var info in longers)
    {
        if (!Rename(info))
            renameFailed.Add(info);
    }

    foreach (var info in renameFailed)
    {
        Console.WriteLine($"***** FAILED Renaming {info.Name} ******");
    }
    Console.WriteLine("Finished");
}

void IsLonger(FileInfo file)
{
    var length = file.FullName.Length;
    Console.WriteLine($"{length}: {file.FullName}");
}

bool Rename(FileInfo file)
{
    var fullLength = file.FullName.Length;
    var nameWithOutExtension = Path.GetFileNameWithoutExtension(file.FullName);
    var nameLength = nameWithOutExtension.Length;
    var toRemove = fullLength - maxLength;
    var nameNewTargetLength = nameLength - toRemove;
    if (nameNewTargetLength > 0)
    {
        var newName = nameWithOutExtension.Substring(0, nameNewTargetLength);

        var number = 1;
        var exists = true;
        string newNameWithExtension = null;
        while (exists)
        {
            newNameWithExtension = newName + "~" + number + file.Extension;
            var newFullName = Path.Combine(file.DirectoryName, newNameWithExtension);
            var newfileInfo = new FileInfo(newFullName);
            var newFullNameLength = newFullName.Length;
            number++;
            exists = newfileInfo.Exists;
            if (exists)
            {

            }
        }
        var destination = Path.Combine(file.DirectoryName, newNameWithExtension);
        Console.WriteLine($"Renaming {file.Name} to {newNameWithExtension}");
        File.Move(file.FullName, destination);
        return true;
    }
    else
    {
          Console.WriteLine($"***** FAILED Renaming {file.Name} ******");
        return false;
    }


}

bool DoIfLonger(Func<FileInfo, bool> check, Action<FileInfo> ifTrue, FileInfo file)
{
    if (check(file))
    {
        ifTrue(file);
        return true;
    }
    return false;
}

bool IsLongerThan(FileInfo fileInfo)
{
    var length = fileInfo.FullName.Length;
    return length > maxLength;
}