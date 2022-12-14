namespace Solvers
{

    public class Day07 : ISolver
    {
        private string _inputFile;
        private Directory _fsRoot;
        private Dictionary<string, int> _directorySizes = new Dictionary<string, int>();

        public Day07(string inputFile)
        {
            _inputFile = inputFile;
            _fsRoot = BuildFilesystem();
            AddDirSizesToDictionary(_fsRoot, _directorySizes);
        }

        public string Answer1()
        {
            // find sum of sizes of dirs with size <= 100000
            int total = 0;
            foreach (string dirName in _directorySizes.Keys)
            {
                Console.WriteLine($"size of dir {dirName} = {_directorySizes[dirName]}");
                if (_directorySizes[dirName] <= 100000)
                {
                    Console.WriteLine($"found dir with size <100000: {dirName} {_directorySizes[dirName]}");
                    total += _directorySizes[dirName];
                    Console.WriteLine($"running total = {total}");
                }
            }

            return total.ToString();
        }

        public string Answer2()
        {
            int totalDiskSpace = 70000000;
            int neededDiskSpace = 30000000;
            int currentFreeSpace = totalDiskSpace - _directorySizes["/"];
            int needToDeleteSize = neededDiskSpace - currentFreeSpace;

            List<int> deletionCandidates = new List<int>();

            Console.WriteLine($"Need to delete {needToDeleteSize} bytes, looking for candidates to delete");
            foreach(string dirName in _directorySizes.Keys)
            {
                if (_directorySizes[dirName] >= needToDeleteSize)
                {
                    Console.WriteLine($"Found candidate dir {dirName} with size {_directorySizes[dirName]}");
                    deletionCandidates.Add(_directorySizes[dirName]);
                }
            }

            deletionCandidates.Sort();
            return deletionCandidates.First().ToString();
        }

        // Helper methods

        private Directory BuildFilesystem()
        {
            // init filesystem
            Directory root = new Directory("/", null);
            Directory current = root;

            foreach (string line in File.ReadLines(_inputFile))
            {
                Console.WriteLine("Building filesystem from input file");
                if (line.StartsWith("$ "))
                {
                    // this is a command
                    string command = line.Substring(2);
                    Console.WriteLine($"found command {command}");
                    if (command.StartsWith("cd "))
                    {
                        string targetDir = command.Substring(3);
                        Console.WriteLine($"targetDir={targetDir}");
                        current = targetDir == "/" ? root : ChangeDir(current, targetDir);
                        Console.WriteLine($"current dir name is now {current.Name}");
                    }
                    else if (command.StartsWith("ls"))
                    {
                        Console.WriteLine("found ls command");
                        // nothing to do, the next input line will start the list of files/dirs for the current directory
                    }
                    else
                    {
                        throw new Exception($"Unknown command {command}");
                    }
                }
                else
                {
                    // must be a filename or directory name
                    if (line.StartsWith("dir "))
                    {
                        string dirName = line.Substring(4);
                        Console.WriteLine($"add subdir {dirName}");
                        current.AddDirectory(dirName);
                    }
                    else
                    {
                        // filename and size
                        // example: "1234 foo.bar"
                        int size = int.Parse(line.Split(' ')[0]);
                        string name = line.Split(' ')[1];
                        current.AddFile(name, size);
                        Console.WriteLine($"Add file {name} ({size})");
                    }
                }
            }
            Console.WriteLine("Finished building filesystem from input\n\n");
            return root;
        }

        private void AddDirSizesToDictionary(Directory current, Dictionary<string,int> directorySizes)
        {
            directorySizes[current.FullName] = current.Size;
            foreach (Directory dir in current.Subdirs)
            {
                AddDirSizesToDictionary(dir, directorySizes);
            }
        }

        private Directory ChangeDir(Directory current, string targetDir)
        {
            if (targetDir == "..")
            {
                // change current dir to parent dir
                if (current.Parent != null)
                {
                    return current.Parent;
                }
                else
                {
                    throw new Exception($"{current.Name} has a null parent directory!");
                }
            }
            else
            {
                List<Directory> subDirs = current.Subdirs;
                foreach (Directory subDir in subDirs)
                {
                    if (subDir.Name == targetDir)
                    {
                        return subDir;
                    }
                }
                throw new Exception($"{current.Name} does not have a subdir named {targetDir}");
            }
        }


    }

    class Directory
    {
        private string _name;
        private Directory? _parentDir;
        private List<Directory> _subdirs;
        private List<FileNode> _files;

        public string Name => _name;
        public Directory? Parent => _parentDir;
        public List<Directory> Subdirs => _subdirs;
        public List<FileNode> Files => _files;

        public int Size 
        { 
            get 
            {
                int total = 0;
                foreach (FileNode file in _files)
                {
                    total += file.Size;
                }
                foreach (Directory dir in _subdirs)
                {
                    total += dir.Size;
                }
                return total;
            }
        }

        public string FullName
        {
            get
            {
                if (Parent == null)
                {
                    return Name;
                }
                string parentFullName = Parent.FullName;
                if (parentFullName == "/")
                {
                    return parentFullName + Name;
                }
                else
                {
                    return parentFullName + "/" + Name;
                }
            }
        }

        public Directory(string name, Directory? parentDir)
        {
            _name = name;
            _parentDir = parentDir;
            _subdirs = new List<Directory>();
            _files = new List<FileNode>();
        }

        public void AddDirectory(string dirName)
        {
            if (_subdirs.Find(dir => dir.Name == dirName) == null)
            {
                _subdirs.Add(new Directory(dirName, this));
            }
            else
            {
                throw new Exception($"directory {this.Name} already has a subdir named {dirName}");
            }
        }

        public void AddFile(string name, int size)
        {
            if (_files.Find(file => file.Name == name) == null)
            {
                _files.Add(new FileNode(name, size));
            }
            else
            {
                throw new Exception($"directory {this.Name} already has a file named {name}");
            }
        }

    }

    class FileNode
    {
        private string _name;
        private int _size;

        public string Name => _name;
        public int Size => _size;

        public FileNode(string name, int size)
        {
            _name = name;
            _size = size;
        }
    }

}