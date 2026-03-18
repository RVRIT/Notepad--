using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class AppConfig
{
    public bool ShowTreeView { get; set; } = true;
    public List<OpenFileConfig> OpenFiles { get; set; } = new();
    public string ActiveFilePath { get; set; } = string.Empty;
}

public class OpenFileConfig
{
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsEdited { get; set; }
}


