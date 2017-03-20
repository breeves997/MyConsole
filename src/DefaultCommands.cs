using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MyConsole.Commands
{
    public static class DefaultCommands
    {
        public static string DoSomething(int id, string data)
        {
            return string.Format(
                "I did something to the record Id {0} and save the data {1}", id, data);
        }

        public static string DoSomethingElse(DateTime date)
        {
            return string.Format("I did something else on {0}", date);
        }

        public static string DoSomethingOptional(int id, string data = "No Data Provided")
        {
            var result = string.Format(
                "I did something to the record Id {0} and save the data {1}", id, data);

            if (data == "No Data Provided")
            {
                result = string.Format(
                "I did something to the record Id {0} but the optinal parameter "
                + "was not provided, so I saved the value '{1}'", id, data);
            }
            return result;
        }

        public static string PurgeTempVimFiles(string dirPath, bool _explicit = false, bool recursive = false)
        {
            string rtn = String.Empty;
            //search my common paths by default because I hate typing. Ususally I just want to kill a single directory
            List<string> commonPaths = new List<string> { @"C:\Devl\Products\FPO\Platform\UI-UXBranch\FieldVisor\FieldVisor.WebRole", @"C:\Devl\Products\FPO\Platform\UI-UXBranch\ScadaVisor\ScadaVisor.WebRole",
            @"C:\Devl\Products\FPO\Platform\FieldVisor\FieldVisor.WebRole", @"C:\Devl\Products\FPO\Platform\FieldVisor\FieldVisor.WebRole"};
            List<string> dirPaths = new List<string>();
            if (_explicit)
                dirPaths = commonPaths.Select(x => String.Join(@"\", x, dirPath)).ToList();
            else dirPaths.Add(dirPath);

            var pattern = @"\.(js|un)~$";
            int filesDeleted = 0;
            foreach (var v in dirPaths)
            {
                if (Directory.Exists(v))
                {
                    DirectoryInfo di = new DirectoryInfo(v);
                    IEnumerable<FileInfo> fileList;
                    if (recursive)
                    {
                        fileList = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Where(x => Regex.Match(x.FullName, pattern).Success);
                    }
                    else
                    {
                         fileList = di.GetFiles().Where(x => Regex.Match(x.FullName, pattern).Success);
                    }

                    filesDeleted += fileList.Count(); //assume the file.Delete() always works

                    try
                    {
                        fileList.ToList().ForEach(x => x.Delete());
                    }
                    catch
                    {
                        rtn = String.Format("One of your files is probably locked, cannot delete all the files. Found {0} to delete though", filesDeleted);
                    }
                }

                else return "Directory does not exist";

            }
            return String.Format("We deleted {0} files.", filesDeleted);
        }

    }
}
