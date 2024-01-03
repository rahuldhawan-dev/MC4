#tool nuget:?package=7-Zip.CommandLine&version=18.1.0

#addin nuget:?package=Cake.7zip&version=0.7.0
#addin nuget:?package=ShellProgressBar&version=5.0.0
#addin nuget:?package=Cake.SqlServer&version=2.2.0

using System.Diagnostics;
using System.IO;
using System.Net;
using ShellProgressBar;

using static System.IO.Path;

public static class ContextHelpers
{
    public static bool ShouldGetNew(ICakeContext context)
    {
        return context.HasArgument("new-copy") ? context.Argument<bool>("new-copy") : false;
    }
}

public class ProgressBarCopier
{
    private ProgressBarOptions GetCommonOptions()
    {
        return new ProgressBarOptions {
            ProgressCharacter = '-'
        };
    }

    private ProgressBarOptions GetParentOptions()
    {
        var options = GetCommonOptions();
        options.ForegroundColor = ConsoleColor.Yellow;
        options.BackgroundColor = ConsoleColor.Gray;
        return options;
    }

    private ProgressBarOptions GetChildOptions()
    {
        var options = GetCommonOptions();
        options.ForegroundColor = ConsoleColor.Green;
        options.BackgroundColor = ConsoleColor.DarkRed;
        return options;
    }

    private void CopyFile(string sourceFile, string destFile, IProgressBar pbar)
    {
        // 1MB Buffer
        const int bufferSize = 1024 * 1024;
        byte[] firstBuffer = new byte[bufferSize],
              secondBuffer = new byte[bufferSize];
        var swap = false;
        int progress = 0, reportedProgress = 0, read = 0;
        Task writer = null;
        var stopwatch = Stopwatch.StartNew();
        var originalMessage = pbar.Message;

        using (var source = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
        {
            long len = source.Length;
            float flen = len;

            using (var dest = new FileStream(destFile, FileMode.CreateNew, FileAccess.Write))
            {
                for (long size = 0; size < len; size += read)
                {
                    if ((progress = ((int)((size / flen) * 100))) != reportedProgress)
                    {
                        var perSecond = size / stopwatch.ElapsedMilliseconds * 1000;
                        var remaining = TimeSpan.FromMilliseconds(((len - size) / perSecond) * 1000);
                        var message =
                            $"{originalMessage} - " +
                            $"{perSecond:N0} Bps, " +
                            $"{remaining:hh\\:mm\\:ss} remaining, " +
                            $"ETA {DateTime.Now.Add(remaining):hh\\:mm\\:ss}";
                        pbar.Tick(reportedProgress = progress, message);
                    }

                    read = source.Read(swap ? firstBuffer : secondBuffer, 0, bufferSize);
                    writer?.Wait();
                    writer = dest.WriteAsync(swap ? firstBuffer : secondBuffer, 0, read);
                    swap = !swap;
                }

                pbar.Tick(100, $"{originalMessage} - finished");
                writer?.Wait();
            }
        }
    }

    public void CopyFiles(string[] sourceFiles, string destinationDirectory)
    {
        if (sourceFiles.Length < 1)
        {
            return;
        }

        var header =
            $"Copying {sourceFiles.Length} files from '{GetDirectoryName(sourceFiles[0])}'...";

        using (var pbar = new ProgressBar(sourceFiles.Length, header, GetParentOptions()))
        {
            foreach (var source in sourceFiles)
            {
                var fileName = GetFileName(source);
                using (var childPbar = pbar.Spawn(100, fileName, GetChildOptions()))
                {
                    CopyFile(source, Combine(destinationDirectory, fileName), childPbar);
                    pbar.Tick();
                }
            }
        }
    }
}

public class ArchiveExtractor
{
    private readonly ICakeContext _context;

    public ArchiveExtractor(ICakeContext context)
    {
        _context = context;
    }

    public void ExtractArchive(string archivePath, string destinationPath)
    {
        _context.Information($"Decompressing file '{archivePath}'...");
        _context.SevenZip(s => s
            .InExtractMode()
            .WithArchive(archivePath)
            .WithoutFullPathExtraction()
            .WithOutputDirectory(destinationPath));
    }
}

public class FileService
{
    public static readonly string LOCAL_PATH = GetFullPath(Combine("\\", "solutions", "nogit", "Sql Backups"));
    public static readonly string REMOTE_PATH = Combine("\\\\mapcallnp01.amwaternp.net", "c$", "solutions", "nogit", "Current");
    public static readonly string LOCAL_TEMP_PATH = Combine(LOCAL_PATH, "tmp");

    private readonly ICakeContext _context;

    public FileService(ICakeContext context)
    {
        _context = context;
    }

    public void EnsureLocalDirectories()
    {
        if (!_context.DirectoryExists(LOCAL_TEMP_PATH))
        {
            _context.CreateDirectory(LOCAL_TEMP_PATH);
        }
    }

    public string[] GetLatestRemoteBackups()
    {
        var baks = System.IO.Directory.GetFiles(REMOTE_PATH, "*.bak*");

        if (baks.Count() < 1)
        {
            throw new Exception($"No remote backup files found in '{REMOTE_PATH}'.");
        }

        return baks;
    }

    public string GetCurrentLocalBak()
    {
        var baks = System.IO.Directory.GetFiles(LOCAL_PATH, "*.bak");

        if (baks.Count() > 1)
        {
            throw new Exception($"More than one current local backup file found in '{LOCAL_PATH}'.");
        }

        return baks.Count() < 1 ? null : baks[0];
    }

    public string DownloadAndUnpackRemoteBackup(string currentLocalBak)
    {
        var baks = GetLatestRemoteBackups();
        var firstBakName = GetFileName(baks[0]);
        var firstBak = Combine(LOCAL_TEMP_PATH, firstBakName);

        new ProgressBarCopier().CopyFiles(baks, LOCAL_TEMP_PATH);
        new ArchiveExtractor(_context).ExtractArchive(firstBak, LOCAL_TEMP_PATH);

        if (!string.IsNullOrWhiteSpace(currentLocalBak))
        {
            _context.Information($"Deleting old backup at '{currentLocalBak}'...");
            _context.DeleteFile(currentLocalBak);
        }

        _context.CopyFiles(Combine(LOCAL_TEMP_PATH, "*.bak"), LOCAL_PATH);
        return Combine(LOCAL_PATH, firstBakName.Replace(".001", ""));
    }

    public void Cleanup()
    {
        _context.Information($"Deleting temporary directory '{LOCAL_TEMP_PATH}'...");
        _context.DeleteDirectory(LOCAL_TEMP_PATH, new DeleteDirectorySettings {
                Recursive = true,
                Force = true
            });
    }
}

public class SqlService
{
    public static readonly string FIX_ORPHAN_SCRIPT = $@"
USE [{DATABASE}];
EXEC sp_change_users_login 'Auto_Fix', 'mapcalldevuser';";

    public const string DB_HOST = "localhost";
    public const string DATABASE = "mapcalldev";
    public const string CONNECTION_STRING =
            "Data Source=" + DB_HOST + ";Integrated Security=true";

    private readonly ICakeContext _context;

    public SqlService(ICakeContext context)
    {
        _context = context;
        _context.SetSqlCommandTimeout(1600);
    }

    private void FixOrphans()
    {
        _context.Information($"Fixing orphaned sql logins...");
        _context.ExecuteSqlCommand(CONNECTION_STRING, FIX_ORPHAN_SCRIPT);
    }

    public void DropExistingDatabase()
    {
        _context.Information($"Dropping database [{DATABASE}]...");
        _context.DropDatabase(CONNECTION_STRING, DATABASE);
    }

    public void RestoreDatabase(string backupPath)
    {
        _context.Information($"Restoring database [{DATABASE}] from file '{backupPath}'...");
        _context.RestoreSqlBackup(CONNECTION_STRING, backupPath, new RestoreSqlBackupSettings {
                WithReplace = true
            });
    }

    public void ExtraPostRestoreSteps()
    {
        FixOrphans();
    }
}

Task("Data-Import")
    .Description($"Restore a development database to {SqlService.DATABASE}.")
    .Does(() => {
        var fileService = new FileService(Context);
        var sqlService = new SqlService(Context);
        var getNew = ContextHelpers.ShouldGetNew(Context);

        try
        {
            fileService.EnsureLocalDirectories();

            var currentLocalBak = fileService.GetCurrentLocalBak();
            var bakToUse = currentLocalBak;

            if (string.IsNullOrWhiteSpace(currentLocalBak) || getNew)
            {
                bakToUse = fileService.DownloadAndUnpackRemoteBackup(currentLocalBak);
            }

            sqlService.DropExistingDatabase();
            sqlService.RestoreDatabase(bakToUse);
            sqlService.ExtraPostRestoreSteps();
        }
        finally
        {
            if (getNew)
            {
                fileService.Cleanup();
            }
        }
    });
